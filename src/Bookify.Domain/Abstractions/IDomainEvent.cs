using MediatR;

namespace Bookify.Domain.Abstractions;

// INotification is for Publish/Subscribe pattern of MediatR
public interface IDomainEvent : INotification
{
}