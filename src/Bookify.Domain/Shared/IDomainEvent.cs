using Mediator;

namespace Bookify.Domain.Shared;

// Mediator INotification is for Publish/Subscribe pattern
public interface IDomainEvent : INotification
{
}