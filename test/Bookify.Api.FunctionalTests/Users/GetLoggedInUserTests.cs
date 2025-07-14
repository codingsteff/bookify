using System.Net.Http.Headers;
using Bookify.Api.FunctionalTests.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Bookify.Api.FunctionalTests;

public class GetLoggedInUserTests : BaseIntegrationTest
{
    public GetLoggedInUserTests(FunctionalTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Get_ShouldReturnUnauthorized_WhenAccessTokenIsMissing()
    {
        // Act
        var response = await HttpClient.GetAsync("/api/v1/users/me", cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_ShouldReturnUser_WhenAccessTokenIsValid()
    {
        // Arrange
        var token = await GetAccessToken();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

        // Act
        var user = await HttpClient.GetAsync("/api/v1/users/me", cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        user.ShouldNotBeNull();
    }

}
