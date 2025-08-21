using Bookify.Application.Bookings.RejectBooking;
using Bookify.Domain.Bookings;
using Mediator;

namespace Bookify.Api.Endpoints.Bookings;

internal sealed partial class BookingsEndpoints
{
    private static async Task<IResult> RejectBooking(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        var command = new RejectBookingCommand(id);

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
