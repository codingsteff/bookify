using Bookify.Api.Endpoints.Users;

namespace Bookify.Api.FunctionalTests.Users;

internal static class UserData
{
    public static RegisterUserRequest RegisterTestUserRequest => new("functional-test@user.com", "first", "last", "123456");
}