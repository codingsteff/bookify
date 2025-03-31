namespace Bookify.Api.Endpoints.Bookings;

// Instead of using directly the ReserveBookingCommand, we can use a record to represent the request
// We don't want to expose our internal Command (loosely coupled)
public sealed record ReserveBookingRequest(
    Guid ApartmentId,
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate);