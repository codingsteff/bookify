using Bookify.Domain.Shared;

namespace Bookify.Application.Abstractions.Messaging;

public interface ICommand : Mediator.IRequest<Result>, IBaseCommand
{
}

public interface ICommand<TReponse> : Mediator.IRequest<Result<TReponse>>, IBaseCommand
{
}

// for apply generic constraints in behavior pipelines (middlewares)
public interface IBaseCommand : Mediator.IMessage
{
}