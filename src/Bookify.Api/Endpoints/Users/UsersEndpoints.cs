using Bookify.Application.Users.GetLoggedInUser;
using Bookify.Application.Users.LogInUser;
using Bookify.Application.Users.RegisterUser;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Bookify.Api.Endpoints.Users;

internal sealed class UsersEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("users").HasApiVersion(ApiVersions.V2).HasDeprecatedApiVersion(ApiVersions.V1);

        MapAuthorizedEndpoints(group);
        MapAnonymousEndpoints(group);
    }

    private static void MapAuthorizedEndpoints(IEndpointRouteBuilder app)
    {
        var authEndpoints = app.MapGroup("").RequireAuthorization(Permissions.UsersRead);

        authEndpoints.MapGet("me", GetLoggedInUserV1).MapToApiVersion(ApiVersions.V1).WithName(nameof(GetLoggedInUserV1));

        authEndpoints.MapGet("me", GetLoggedInUser).MapToApiVersion(ApiVersions.V2).WithName(nameof(GetLoggedInUser));
    }

    private static void MapAnonymousEndpoints(IEndpointRouteBuilder app)
    {
        var anonymousEndpoints = app.MapGroup("").AllowAnonymous();

        anonymousEndpoints.MapPost("register", RegisterUser).WithName(nameof(RegisterUser));

        anonymousEndpoints.MapPost("login", LogInUser).WithName(nameof(LogInUser));
    }

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

    private static async Task<Results<Ok<Guid>, BadRequest<Error>>> RegisterUser(RegisterUserRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return TypedResults.BadRequest(result.Error);
        }

        return TypedResults.Ok(result.Value);
    }

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