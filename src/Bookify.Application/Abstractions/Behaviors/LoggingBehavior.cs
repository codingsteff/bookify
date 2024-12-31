using Bookify.Application.Abstractions.Messaging;
using Microsoft.Extensions.Logging;
using MediatR;

namespace Bookify.Application.Abstractions.Behaviors;
public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand // Loggingn only for commands, queries should be as fast as possible
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {

        var name = request.GetType().Name;

        try
        {

            _logger.LogInformation("Executing command {Command}", name);
            var response = await next();
            _logger.LogInformation("Command {Command} processed successfully", name);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Command {Command} failed", name);
            throw;
        }
    }
}