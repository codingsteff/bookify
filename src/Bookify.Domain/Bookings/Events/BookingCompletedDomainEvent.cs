using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings.Events;

public sealed record BookingCompletedDomainEvent(Guid BookingId) : IDomainEvent;