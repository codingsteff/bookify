using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.Users;
using Bookify.Domain.Users.Events;
using Shouldly;

namespace Bookify.Domain.UnitTests.Users;

public class UserTests : BaseTest
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);

        user.FirstName.ShouldBe(UserData.FirstName);
        user.LastName.ShouldBe(UserData.LastName);
        user.Email.ShouldBe(UserData.Email);
    }

    [Fact]
    public void Create_Should_RaiseUserCreatedDomainEvent()
    {
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);

        var domainEvent = AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);

        domainEvent.UserId.ShouldBe(user.Id);
    }

    [Fact]
    public void Create_Should_AddRegisteredRoleToUser()
    {
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);

        user.Roles.ShouldContain(Role.Registered);
    }

    [Fact]
    public void Create_Should_GenerateUniqueId()
    {
        // Act
        var user1 = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var user2 = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);

        // Assert
        user1.Id.ShouldNotBe(user2.Id);
        user1.Id.ShouldNotBe(Guid.Empty);
        user2.Id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public void Create_Should_InitializeIdentityIdAsEmpty()
    {
        // Act
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);

        // Assert
        user.IdentityId.ShouldBe(string.Empty);
    }

    [Fact]
    public void SetIdentityId_Should_SetIdentityIdCorrectly()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var identityId = "auth0|123456789";

        // Act
        user.SetIdentityId(identityId);

        // Assert
        user.IdentityId.ShouldBe(identityId);
    }

    [Fact]
    public void Roles_Should_ReturnReadOnlyCollection()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);

        // Act
        var roles = user.Roles;

        // Assert
        roles.ShouldBeOfType<System.Collections.ObjectModel.ReadOnlyCollection<Role>>();
        roles.ShouldContain(Role.Registered);
    }

}
