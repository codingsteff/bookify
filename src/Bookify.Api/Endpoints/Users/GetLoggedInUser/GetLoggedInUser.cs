using Bookify.Application.Users.GetLoggedInUser;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Bookify.Api.Endpoints.Users;

internal sealed partial class UsersEndpoints : IEndpoint
{
    private static async Task<Ok<UserResponse>> GetLoggedInUserV1(ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetLoggedInUserQuery();

        var result = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(result.Value);
    }

    private static async Task<Ok<UserResponse>> GetLoggedInUser(ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetLoggedInUserQuery();

        var result = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(result.Value);
    }

}