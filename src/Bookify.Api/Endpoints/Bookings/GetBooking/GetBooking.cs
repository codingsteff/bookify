using Bookify.Application.Bookings.GetBooking;
using Mediator;

namespace Bookify.Api.Endpoints.Bookings;

internal sealed partial class BookingsEndpoints 
{
    private static async Task<IResult> GetBooking(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetBookingQuery(id);

        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : TypedResults.NotFound();
    }

}