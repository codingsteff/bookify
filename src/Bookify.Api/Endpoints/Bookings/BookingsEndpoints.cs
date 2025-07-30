namespace Bookify.Api.Endpoints.Bookings;

internal sealed partial class BookingsEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("bookings").RequireAuthorization();

        group.MapGet("{id}", GetBooking).WithName(nameof(GetBooking));

        group.MapPost("", ReserveBooking).WithName(nameof(ReserveBooking));
        
        group.MapPut("{id}/reject", RejectBooking).WithName(nameof(RejectBooking));

        group.MapDelete("{id}", CancelBooking).WithName(nameof(CancelBooking));
    }

}