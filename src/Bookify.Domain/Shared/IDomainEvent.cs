using MediatR;

namespace Bookify.Domain.Shared;

// MediatR INotification is for Publish/Subscribe pattern
public interface IDomainEvent : INotification
{
}