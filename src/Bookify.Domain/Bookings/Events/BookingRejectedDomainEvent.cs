using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings.Events;

public sealed record BookingRejectedDomainEvent(Guid BookingId) : IDomainEvent;