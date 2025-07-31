using Bookify.Application.Users.SearchUsers;
using MediatR;

namespace Bookify.Api.Endpoints.Users;

internal sealed partial class UsersEndpoints
{
    private static async Task<IResult> SearchUsers(string? searchTerm, string? exactMatch, ISender sender, CancellationToken cancellationToken)
    {
        var query = new SearchUsersQuery(searchTerm, exactMatch);

        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : TypedResults.BadRequest(result.Error);
    }
}
