using System.Data;
using System.Text.Json;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Data;
using Bookify.Domain.Shared;
using Bookify.Infrastructure.Data;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace Bookify.Infrastructure.Outbox;

[DisallowConcurrentExecution] // ensures that only one instance of the job is running at a time
internal sealed class OutboxProcessorJob : IJob
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = {new JsonPolymorphicConverter<IDomainEvent>()}
    };

    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IPublisher _publisher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly OutboxOptions _outboxOptions;
    private readonly ILogger<OutboxProcessorJob> _logger;

    public OutboxProcessorJob(
        ISqlConnectionFactory sqlConnectionFactory,
        IPublisher publisher,
        IDateTimeProvider dateTimeProvider,
        IOptions<OutboxOptions> outboxOptions,
        ILogger<OutboxProcessorJob> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _publisher = publisher;
        _dateTimeProvider = dateTimeProvider;
        _outboxOptions = outboxOptions.Value;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Beginning to process outbox messages");

        using var connection = _sqlConnectionFactory.CreateConnection();
        using var transaction = connection.BeginTransaction();

        var outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        foreach (var outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                var domainEvent = JsonSerializer.Deserialize<IDomainEvent>(outboxMessage.Content, JsonSerializerOptions)!;

                await _publisher.Publish(domainEvent, context.CancellationToken); // Triggers the respective domain event handlers (in Application layer)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while processing outbox message {MessageId}", outboxMessage.Id);
                exception = ex;
            }

            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }

        transaction.Commit();
        _logger.LogInformation("Completed processing outbox messages");
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(IDbConnection connection, IDbTransaction transaction)
    {
        // query messages that have not been processed yet
        // SELECT-FOR UPDATE: locks the rows during the transaction so that they cannot be changed by other processes
        // SKIP LOCKED: skips the locked rows and only returns the unlocked rows
        // TODO: try out with SKIP LOCKED
        var sql = $"""
                SELECT id, content
                FROM outbox_messages
                WHERE processed_on_utc IS NULL
                ORDER BY occurred_on_utc
                LIMIT {_outboxOptions.BatchSize}
                FOR UPDATE -- SKIP LOCKED;
                """;

        var outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);

        return outboxMessages.ToList();
    }

    private async Task UpdateOutboxMessageAsync(IDbConnection connection, IDbTransaction transaction, OutboxMessageResponse outboxMessage, Exception? exception)
    {
        const string sql = @"
                            UPDATE outbox_messages
                            SET processed_on_utc = @ProcessedOnUtc,
                                error = @Error
                            WHERE id = @Id";

        await connection.ExecuteAsync(sql, new
        {
            outboxMessage.Id,
            ProcessedOnUtc = _dateTimeProvider.UtcNow,
            Error = exception?.ToString()
        },
        transaction: transaction);
    }

    internal sealed record OutboxMessageResponse(Guid Id, string Content);
}