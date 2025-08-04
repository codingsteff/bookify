using Bookify.Application.Users.SearchUsers;

namespace Bookify.Application.Apartments;

public interface IApartmentReadRepository
{
    Task<IReadOnlyList<UserResponse>> SearchUser(string? searchTerm, string? exactMatch, CancellationToken cancellationToken = default);
}
