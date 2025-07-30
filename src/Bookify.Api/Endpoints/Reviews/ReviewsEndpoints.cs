namespace Bookify.Api.Endpoints.Reviews;

internal sealed partial class ReviewsEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("reviews").RequireAuthorization();

        group.MapPost("", AddReview).WithName(nameof(AddReview));
    }

}