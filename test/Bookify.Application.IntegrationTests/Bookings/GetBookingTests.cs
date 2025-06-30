using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.IntegrationTests.Infrastructure;
using Bookify.Domain.Bookings;
using Shouldly;

namespace Bookify.Application.IntegrationTests.Bookings;

public class GetBookingTests : BaseIntegrationTest
{
    private static readonly Guid BookingId = Guid.NewGuid();

    public GetBookingTests(IntegrationTestWebAppFactory webAppFactory) 
        : base(webAppFactory)
    {
    }

    [Fact]
    public async Task GetBooking_ShouldReturnFailure_WhenBookingIsNotFound()
    {
        // Arrange
        var command = new GetBookingQuery(BookingId);

        // Act
        var result = await Sender.Send(command, TestContext.Current.CancellationToken);

        // Assert
        result.Error.ShouldBe(BookingErrors.NotFound);
    }
}