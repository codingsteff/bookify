using Bookify.Api.Endpoints.Users;
using Bookify.Api.FunctionalTests.Infrastructure;
using Bookify.Api.FunctionalTests.Users;

namespace Bookify.Api.FunctionalTests;

public class LoginUserTests : BaseIntegrationTest
{
    public LoginUserTests(FunctionalTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new LogInUserRequest("notexist@test.com", "12345");

        // Act
        var response = await HttpClient.PostAsJsonAsync("/api/v1/users/login", request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WhenUserExists()
    {
        // Arrange
        var request = new LogInUserRequest(UserData.RegisterTestUserRequest.Email, UserData.RegisterTestUserRequest.Password);

        // Act
        var response = await HttpClient.PostAsJsonAsync("/api/v1/users/login", request, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

}
