using Bookify.Application.Bookings.ReserveBooking;
using Mediator;

namespace Bookify.Api.Endpoints.Bookings;

internal sealed partial class BookingsEndpoints 
{
    private static async Task<IResult> ReserveBooking(ReserveBookingRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new ReserveBookingCommand(
            request.ApartmentId,
            request.UserId,
            request.StartDate,
            request.EndDate);

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return TypedResults.BadRequest(result.Error);
        }

        return TypedResults.CreatedAtRoute(result.Value, nameof(GetBooking), new { id = result.Value });
    }

}