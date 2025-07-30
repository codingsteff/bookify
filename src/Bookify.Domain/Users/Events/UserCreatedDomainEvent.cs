using Bookify.Domain.Shared;

namespace Bookify.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;