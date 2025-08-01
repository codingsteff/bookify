using Bookify.Application.Apartments.SearchApartments;
using MediatR;

namespace Bookify.Api.Endpoints.Apartments;

internal sealed partial class ApartmentsEndpoints 
{
    private static async Task<IResult> SearchApartments(DateOnly startDate, DateOnly endDate, ISender sender, CancellationToken cancellationToken)
    {
        var query = new SearchApartmentsQuery(startDate, endDate);

        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : TypedResults.BadRequest(result.Error);
    }
    
}