namespace Bookify.Api.Endpoints.Users;

internal sealed partial class UsersEndpoints : IEndpoint
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

}