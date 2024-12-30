using MediatR;

namespace Bookify.Domain.Abstractions;

// MediatR INotification is for Publish/Subscribe pattern
public interface IDomainEvent : INotification
{
}