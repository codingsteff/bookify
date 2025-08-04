using Bookify.Application.Apartments;
using Bookify.Application.Users.SearchUsers;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Repositories;

internal sealed class ApartmentReadRepository : IApartmentReadRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ApartmentReadRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<UserResponse>> SearchUser(string? searchTerm, string? exactMatch, CancellationToken cancellationToken)
    {
        var query = _dbContext.Users.AsNoTracking();

        // Apply email filtering based on the provided parameters
        if (!string.IsNullOrWhiteSpace(exactMatch))
        {
            // Exact match for email
            query = query.Where(u => u.Email.Value == exactMatch);
        }
        else if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            // Like search for email (case-insensitive)
            query = query.Where(u => u.Email.Value.Contains(searchTerm));
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