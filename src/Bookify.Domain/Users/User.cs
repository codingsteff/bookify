using Bookify.Domain.Shared;
using Bookify.Domain.Users.Events;

namespace Bookify.Domain.Users;

public class User : Entity
{
    // encapsulate role in the user entity (kind of aggregate)
    private readonly List<Role> _roles = new();

    private User(Guid id, FirstName firstName, LastName lastName, Email email)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    public string IdentityId { get; private set; } = string.Empty; // ID from Identity Provider
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    // Factory method: 
    // 1. hide the constructor to not expose implementation details. for example: Id creation
    // 2. encapsulation
    // 3. introduce side effects and they do not belong to the constructor. for example: domain events
    public static User Create(FirstName firstName, LastName lastName, Email email)
    {
        var user = new User(Guid.NewGuid(), firstName, lastName, email);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        user._roles.Add(Role.Registered);

        return user;
    }

    public void SetIdentityId(string identityId)
    {
        IdentityId = identityId;
    }

#pragma warning disable CS8618 // Non-nullable warning, but required for EF Core
    private User() { }

}