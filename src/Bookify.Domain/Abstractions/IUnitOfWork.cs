namespace Bookify.Domain.Abstractions;

// Implementation in ApplicationDbContext: SaveChangesAsync same signature
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}