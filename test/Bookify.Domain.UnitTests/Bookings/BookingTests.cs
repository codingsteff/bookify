using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.Bookings;
using Shouldly;
using Bookify.Domain.UnitTests.Apartments;
using Bookify.Domain.Shared;
using Bookify.Domain.UnitTests.Users;
using Bookify.Domain.Users;
using Bookify.Domain.Bookings.Events;

namespace Bookify.Domain.UnitTests.Bookings;

public class BookingTests : BaseTest
{
    [Fact]
    public void Reserve_Should_RaiseBookingReservedDomainEvent()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(100, Currency.Eur);
        var apartment = ApartmentData.Create(price);
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var utcNow = DateTime.UtcNow;
        var pricingService = new PricingService();

        // Act
        var booking = Booking.Reserve(apartment, user.Id, duration, utcNow, pricingService);

        // Assert
        var domainEvent = AssertDomainEventWasPublished<BookingReservedDomainEvent>(booking);
        domainEvent.BookingId.ShouldBe(booking.Id);
    }

}