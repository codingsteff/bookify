using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings.Events;

public sealed record BookingConfirmedDomainEvent(Guid BookingId) : IDomainEvent;