using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings.Events;

public sealed record BookingCancelledDomainEvent(Guid BookingId) : IDomainEvent;