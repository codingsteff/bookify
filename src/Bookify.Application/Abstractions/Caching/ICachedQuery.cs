using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Abstractions.Caching;

public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery;

public interface ICachedQuery : Mediator.IMessage // for pipeline constraint in QueryCachingBehavior
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
}