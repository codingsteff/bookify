using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.Bookings;
using Shouldly;
using Bookify.Domain.UnitTests.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Domain.UnitTests.Bookings;

public class PricingServiceTests
{
    [Fact]
    public void CalculatePrice_Should_ReturnCorrectTotalPrice()
    {
        // Arrange
        var price = new Money(100, Currency.Eur);
        var period = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var expectedPrice = new Money(price.Amount * period.LengthInDays, price.Currency);
        var apartment = ApartmentData.Create(price);
        var pricingService = new PricingService();

        // Act
        var pricingDetails = pricingService.CalculatePrice(apartment, period);

        // Assert
        pricingDetails.TotalPrice.ShouldBe(expectedPrice);
    }

    [Fact]
    public void CalculatePrice_Should_ReturnCorrectTotalPrice_WhenCleaningFeeIsIncluded()
    {
        // Arrange
        var price = new Money(100, Currency.Eur);
        var cleaningFee = new Money(25, Currency.Eur);
        var period = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var expectedPrice = new Money(price.Amount * period.LengthInDays + cleaningFee.Amount, price.Currency);
        var apartment = ApartmentData.Create(price, cleaningFee);
        var pricingService = new PricingService();

        // Act
        var pricingDetails = pricingService.CalculatePrice(apartment, period);

        // Assert
        pricingDetails.TotalPrice.ShouldBe(expectedPrice);
    }

}