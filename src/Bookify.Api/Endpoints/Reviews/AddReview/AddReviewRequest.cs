namespace Bookify.Api.Endpoints.Reviews;

public sealed record AddReviewRequest(Guid BookingId, int Rating, string Comment);