namespace Bookify.Domain.Users;

// This class is used to define the roles that can be used in the application

// Prgramatic:
// don't inherit from Entity, because a GUID as Primary Key it's not needed
// it doesn't follow the domain driven design principles this is more a practical way to manage roles
// Roles are not defined in keycloak, they are defined in the system itself for simplicity (we also need to add them to the token)
// Keycloak are only used to authenticate users
public sealed class Role
{
    public static readonly Role Registered = new Role(1, "Registered");
    public Role(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;

    public ICollection<User> Users { get; init; } = new List<User>();
}