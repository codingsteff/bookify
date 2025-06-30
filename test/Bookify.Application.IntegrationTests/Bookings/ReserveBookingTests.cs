using Bookify.Application.Bookings.ReserveBooking;
using Bookify.Application.IntegrationTests.Infrastructure;
using Bookify.Domain.Users;
using Shouldly;

namespace Bookify.Application.IntegrationTests.Bookings;

public class ReserveBookingTests : BaseIntegrationTest
{
    private static readonly Guid ApartmentId = Guid.NewGuid();
    private static readonly Guid UserId = Guid.NewGuid();
    private static readonly DateOnly StartDate = new DateOnly(2024, 1, 1);
    private static readonly DateOnly EndDate = new DateOnly(2024, 1, 10);


    public ReserveBookingTests(IntegrationTestWebAppFactory webAppFactory) : base(webAppFactory)
    {
    }

    [Fact]
    public async Task ReserveBooking_ShouldReturnFailure_WhenUserIsNotFound()
    {
        // Arrange
        var command = new ReserveBookingCommand(ApartmentId, UserId, StartDate, EndDate);
        // Possible to direclty access to DB (DbContext.Users) or call appropriate Command
        
        // Act
        var result = await Sender.Send(command, TestContext.Current.CancellationToken);

        // Assert
        result.Error.ShouldBe(UserErrors.NotFound);
    }

}