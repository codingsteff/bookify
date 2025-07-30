using Bookify.Domain.Bookings;
using Shouldly;
using Bookify.Domain.UnitTests.Apartments;
using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.Common;
using Bookify.Domain.Apartments;

namespace Bookify.Domain.UnitTests.Bookings;

public class PricingServiceTests : BaseTest
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

    [Theory]
    [InlineData(Amenity.GardenView, 0.05)]
    [InlineData(Amenity.MountainView, 0.05)]
    [InlineData(Amenity.AirConditioning, 0.01)]
    public void CalculatePrice_Should_ApplyCorrectUpcharge_ForDifferentAmenities(Amenity amenity, decimal expectedUpchargePercentage)
    {
        // Arrange
        var price = new Money(100, Currency.Eur);
        var cleaningFee = Money.Zero(Currency.Eur);
        var apartment = new Apartment(
            Guid.NewGuid(),
            new Name("Test apartment"),
            new Description("Test description"),
            new Address("Country", "State", "ZipCode", "City", "Street"),
            price,
            cleaningFee,
            [amenity]);
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 6)); // 5 days
        var pricingService = new PricingService();

        // Act
        var pricingDetails = pricingService.CalculatePrice(apartment, duration);

        // Assert
        var expectedPriceForPeriod = 500m; // 100 * 5 days
        var expectedUpcharge = expectedPriceForPeriod * expectedUpchargePercentage;
        pricingDetails.PriceForPeriod.Amount.ShouldBe(expectedPriceForPeriod);
        pricingDetails.AmenitiesUpCharge.Amount.ShouldBe(expectedUpcharge);
        pricingDetails.TotalPrice.Amount.ShouldBe(expectedPriceForPeriod + expectedUpcharge);
    }

    [Fact]
    public void CalculatePrice_Should_ApplyCorrectUpcharge_WhenApartmentHasMultipleAmenities()
    {
        // Arrange
        var price = new Money(100, Currency.Eur);
        var cleaningFee = Money.Zero(Currency.Eur);
        var apartment = new Apartment(
            Guid.NewGuid(),
            new Name("Test apartment"),
            new Description("Test description"),
            new Address("Country", "State", "ZipCode", "City", "Street"),
            price,
            cleaningFee,
            [Amenity.GardenView, Amenity.AirConditioning, Amenity.Parking]);
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 6)); // 5 days
        var pricingService = new PricingService();

        // Act
        var pricingDetails = pricingService.CalculatePrice(apartment, duration);

        // Assert
        var expectedPriceForPeriod = 500m; // 100 * 5 days
        var expectedUpchargePercentage = 0.05m + 0.01m + 0.01m; // 7% total
        var expectedUpcharge = expectedPriceForPeriod * expectedUpchargePercentage;
        pricingDetails.AmenitiesUpCharge.Amount.ShouldBe(expectedUpcharge);
        pricingDetails.TotalPrice.Amount.ShouldBe(expectedPriceForPeriod + expectedUpcharge);
    }

    [Fact]
    public void CalculatePrice_Should_NotApplyUpcharge_WhenApartmentHasNonChargeableAmenities()
    {
        // Arrange
        var price = new Money(100, Currency.Eur);
        var cleaningFee = Money.Zero(Currency.Eur);
        var apartment = new Apartment(
            Guid.NewGuid(),
            new Name("Test apartment"),
            new Description("Test description"),
            new Address("Country", "State", "ZipCode", "City", "Street"),
            price,
            cleaningFee,
            [Amenity.WiFi, Amenity.PetFriendly, Amenity.SwimmingPool]);
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 6)); // 5 days
        var pricingService = new PricingService();

        // Act
        var pricingDetails = pricingService.CalculatePrice(apartment, duration);

        // Assert
        pricingDetails.AmenitiesUpCharge.Amount.ShouldBe(0);
    }

}