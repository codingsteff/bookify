using Bookify.Application.Bookings.GetBooking;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Bookify.Api.Endpoints.Bookings;

internal sealed partial class BookingsEndpoints 
{
    private static async Task<Results<Ok<BookingResponse>, NotFound>> GetBooking(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetBookingQuery(id);

        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : TypedResults.NotFound();
    }

}