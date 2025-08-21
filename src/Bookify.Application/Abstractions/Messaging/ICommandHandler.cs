using Bookify.Domain.Shared;

namespace Bookify.Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand> : Mediator.IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse> : Mediator.IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}