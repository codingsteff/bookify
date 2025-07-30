using Bookify.Application.Users.LogInUser;
using MediatR;

namespace Bookify.Api.Endpoints.Users;

internal sealed partial class UsersEndpoints : IEndpoint
{
    // login user: api act as a proxy to keycloak
    private static async Task<IResult> LogInUser(LogInUserRequest request, ISender sender, CancellationToken cancellationToken)
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