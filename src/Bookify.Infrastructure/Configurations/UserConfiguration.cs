using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(user => user.Id);

        builder.Property(user => user.FirstName)
            .HasMaxLength(255)
            .HasConversion(firstName => firstName.Value, value => new FirstName(value));

        builder.Property(user => user.LastName)
            .HasMaxLength(255)
            .HasConversion(lastName => lastName.Value, value => new LastName(value));

        // Value Object as Complex Property so we can use it in queries
        builder.ComplexProperty(user => user.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Value)
                .HasMaxLength(400)
                .HasColumnName("email"); // otherwise email_value
            // emailBuilder.HasIndex(e => e.Value).IsUnique();
        });
        // when using complex properties: indexes are currently not supported
        // https://github.com/dotnet/efcore/issues/31246
        // builder.HasIndex(user => user.Email).IsUnique();
        // builder.HasIndex(user => user.Email).HasDatabaseName("ix_users_email").IsUnique();
        // Workaround: create index in migration

        builder.HasIndex(user => user.IdentityId).IsUnique();
    }
}