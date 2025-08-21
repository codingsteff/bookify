using Bookify.Domain.Shared;

namespace Bookify.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : Mediator.IRequest<Result<TResponse>>;