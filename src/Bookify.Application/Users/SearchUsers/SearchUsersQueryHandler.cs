using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Users.SearchUsers;

internal sealed class SearchUsersQueryHandler : IQueryHandler<SearchUsersQuery, IReadOnlyList<UserResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public SearchUsersQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<IReadOnlyList<UserResponse>>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Users.AsNoTracking();

        // Apply email filtering based on the provided parameters
        if (!string.IsNullOrWhiteSpace(request.ExactMatch))
        {
            // Exact match for email
            query = query.Where(u => u.Email.Value == request.ExactMatch);
        }
        else if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            // Like search for email (case-insensitive)
            query = query.Where(u => u.Email.Value.Contains(request.SearchTerm));
        }

        var users = await query
            .OrderBy(u => u.Email.Value)
            .Select(u => new UserResponse
            {
                Id = u.Id,
                FirstName = u.FirstName.Value,
                LastName = u.LastName.Value,
                Email = u.Email.Value
            })
            .ToListAsync(cancellationToken);

        return users;
    }
}
