namespace Bookify.Domain.Users;

public sealed class Permission
{
    public static readonly Permission UsersRead = new Permission(1, "users:read");
    public Permission(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}