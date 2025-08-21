using Bookify.Application.Reviews.AddReview;
using Mediator;

namespace Bookify.Api.Endpoints.Reviews;

internal sealed partial class ReviewsEndpoints 
{
    public static async Task<IResult> AddReview(AddReviewRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new AddReviewCommand(request.BookingId, request.Rating, request.Comment);

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? TypedResults.Ok() : TypedResults.BadRequest(result.Error);
    }

}