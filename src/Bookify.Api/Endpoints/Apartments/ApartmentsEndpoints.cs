namespace Bookify.Api.Endpoints.Apartments;

internal sealed partial class ApartmentsEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("apartments");

        group.MapGet("", SearchApartments).WithName(nameof(SearchApartments));
    }

}