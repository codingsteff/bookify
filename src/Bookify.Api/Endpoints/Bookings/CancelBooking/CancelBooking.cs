using Bookify.Application.Bookings.CancelBooking;
using Bookify.Domain.Bookings;
using Mediator;

namespace Bookify.Api.Endpoints.Bookings;

internal sealed partial class BookingsEndpoints
{
    private static async Task<IResult> CancelBooking(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new CancelBookingCommand(id);

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error == BookingErrors.NotFound
                ? TypedResults.NotFound()
                : TypedResults.BadRequest(result.Error);
        }

        return TypedResults.NoContent();
    }
}
