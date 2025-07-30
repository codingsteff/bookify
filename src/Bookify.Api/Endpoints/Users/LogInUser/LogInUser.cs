using Bookify.Application.Users.LogInUser;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Bookify.Api.Endpoints.Users;

internal sealed partial class UsersEndpoints : IEndpoint
{
    // login user: api act as a proxy to keycloak
    private static async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult>> LogInUser(LogInUserRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new LogInUserCommand(request.Email, request.Password);

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return TypedResults.Unauthorized();
        }

        return TypedResults.Ok(result.Value);
    }

}