using Bookify.Application.Apartments;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Shared;

namespace Bookify.Application.Users.SearchUsers;

internal sealed class SearchUsersQueryHandler : IQueryHandler<SearchUsersQuery, IReadOnlyList<UserResponse>>
{
    private readonly IApartmentReadRepository _repository;

    public SearchUsersQueryHandler(IApartmentReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IReadOnlyList<UserResponse>>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _repository.SearchUser(request.SearchTerm, request.ExactMatch, cancellationToken);

        return Result.Success(users);
    }
}