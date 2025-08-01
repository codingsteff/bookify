using System.Text.Json;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Data;
using Bookify.Application.Exceptions;
using Bookify.Infrastructure.Data;
using Bookify.Domain.Shared;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;


namespace Bookify.Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork, IApplicationDbContext
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = { new JsonPolymorphicConverter<IDomainEvent>() }
    };

    private readonly IDateTimeProvider _dateTimeProvider;

    public DbSet<Apartment> Apartments { get; private set; }

    public DbSet<Booking> Bookings { get; private set; }

    public DbSet<Review> Reviews { get; private set; }

    public DbSet<User> Users { get; private set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeProvider dateTimeProvider)
        : base(options)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            AddDomainEventsAsOutboxMessages();
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // ConcurrencyException instead of DbUpdateConcurrencyException can go up to the Application layer (hide ef core in application layer)
            throw new ConcurrencyException("Concurrency exception occured", ex);
        }
    }

    private void AddDomainEventsAsOutboxMessages()
    {
        var outboxMessages = ChangeTracker
            .Entries<Entity>() // only entries with base type Entity from Domain
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage(
                Guid.NewGuid(),
                _dateTimeProvider.UtcNow,
                domainEvent.GetType().Name,
                JsonSerializer.Serialize(domainEvent, JsonSerializerOptions))
            )
            .ToList();

        AddRange(outboxMessages);
    }
}