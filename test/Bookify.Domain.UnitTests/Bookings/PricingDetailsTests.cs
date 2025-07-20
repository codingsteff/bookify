using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.Bookings;
using Bookify.Domain.Shared;
using Shouldly;

namespace Bookify.Domain.UnitTests.Bookings;

public class PricingDetailsTests : BaseTest
{
    [Fact]
    public void Constructor_Should_SetPropertiesCorrectly()
    {
        // Arrange
        var priceForPeriod = new Money(500, Currency.Eur);
        var cleaningFee = new Money(30, Currency.Eur);
        var amenitiesUpCharge = new Money(25, Currency.Eur);
        var totalPrice = new Money(555, Currency.Eur);

        // Act
        var pricingDetails = new PricingDetails(priceForPeriod, cleaningFee, amenitiesUpCharge, totalPrice);

        // Assert
        pricingDetails.PriceForPeriod.ShouldBe(priceForPeriod);
        pricingDetails.CleaningFee.ShouldBe(cleaningFee);
        pricingDetails.AmenitiesUpCharge.ShouldBe(amenitiesUpCharge);
        pricingDetails.TotalPrice.ShouldBe(totalPrice);
    }

    [Fact]
    public void Equals_Should_ReturnTrue_WhenAllPropertiesAreEqual()
    {
        // Arrange
        var priceForPeriod = new Money(500, Currency.Eur);
        var cleaningFee = new Money(30, Currency.Eur);
        var amenitiesUpCharge = new Money(25, Currency.Eur);
        var totalPrice = new Money(555, Currency.Eur);

        var pricingDetails1 = new PricingDetails(priceForPeriod, cleaningFee, amenitiesUpCharge, totalPrice);
        var pricingDetails2 = new PricingDetails(priceForPeriod, cleaningFee, amenitiesUpCharge, totalPrice);

        // Act & Assert
        pricingDetails1.ShouldBe(pricingDetails2);
    }

    [Fact]
    public void Equals_Should_ReturnFalse_WhenPropertiesAreDifferent()
    {
        // Arrange
        var priceForPeriod1 = new Money(500, Currency.Eur);
        var priceForPeriod2 = new Money(600, Currency.Eur);
        var cleaningFee = new Money(30, Currency.Eur);
        var amenitiesUpCharge = new Money(25, Currency.Eur);
        var totalPrice1 = new Money(555, Currency.Eur);
        var totalPrice2 = new Money(655, Currency.Eur);

        var pricingDetails1 = new PricingDetails(priceForPeriod1, cleaningFee, amenitiesUpCharge, totalPrice1);
        var pricingDetails2 = new PricingDetails(priceForPeriod2, cleaningFee, amenitiesUpCharge, totalPrice2);

        // Act & Assert
        pricingDetails1.ShouldNotBe(pricingDetails2);
    }

    [Fact]
    public void Constructor_Should_AcceptZeroValues()
    {
        // Arrange
        var priceForPeriod = new Money(100, Currency.Eur);
        var cleaningFee = Money.Zero(Currency.Eur);
        var amenitiesUpCharge = Money.Zero(Currency.Eur);
        var totalPrice = new Money(100, Currency.Eur);

        // Act
        var pricingDetails = new PricingDetails(priceForPeriod, cleaningFee, amenitiesUpCharge, totalPrice);

        // Assert
        pricingDetails.CleaningFee.IsZero().ShouldBeTrue();
        pricingDetails.AmenitiesUpCharge.IsZero().ShouldBeTrue();
        pricingDetails.TotalPrice.Amount.ShouldBe(100);
    }
}
