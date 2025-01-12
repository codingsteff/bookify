using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    override public void Add(User user)
    {
        foreach(var role in user.Roles)
        {
            // Tell ef core that the role is already in the database and it should not be inserted
            DbContext.Attach(role); 
        }
        base.Add(user);
    }
}