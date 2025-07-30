using Bookify.Domain.Shared;
using MediatR;

namespace Bookify.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}