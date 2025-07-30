using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.Common;
using Shouldly;

namespace Bookify.Domain.UnitTests.Shared;

public class MoneyTests : BaseTest
{
    [Fact]
    public void Addition_Should_ReturnCorrectSum_WhenCurrenciesAreEqual()
    {
        // Arrange
        var money1 = new Money(100m, Currency.Eur);
        var money2 = new Money(50m, Currency.Eur);

        // Act
        var result = money1 + money2;

        // Assert
        result.Amount.ShouldBe(150m);
        result.Currency.ShouldBe(Currency.Eur);
    }

    [Fact]
    public void Addition_Should_ThrowInvalidOperationException_WhenCurrenciesAreDifferent()
    {
        // Arrange
        var money1 = new Money(100m, Currency.Eur);
        var money2 = new Money(50m, Currency.Usd);

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => money1 + money2)
            .Message.ShouldBe("Currencies have to be equal");
    }

    [Fact]
    public void Zero_Should_ReturnZeroAmountWithNoCurrency()
    {
        // Act
        var zero = Money.Zero();

        // Assert
        zero.Amount.ShouldBe(0m);
    }

    [Fact]
    public void Zero_Should_ReturnZeroAmountWithSpecifiedCurrency()
    {
        // Act
        var zero = Money.Zero(Currency.Eur);

        // Assert
        zero.Amount.ShouldBe(0m);
        zero.Currency.ShouldBe(Currency.Eur);
    }

    [Fact]
    public void IsZero_Should_ReturnTrue_WhenAmountIsZero()
    {
        // Arrange
        var money = Money.Zero(Currency.Eur);

        // Act
        var isZero = money.IsZero();

        // Assert
        isZero.ShouldBeTrue();
    }

    [Fact]
    public void IsZero_Should_ReturnFalse_WhenAmountIsNotZero()
    {
        // Arrange
        var money = new Money(100m, Currency.Eur);

        // Act
        var isZero = money.IsZero();

        // Assert
        isZero.ShouldBeFalse();
    }

    [Fact]
    public void Constructor_Should_SetPropertiesCorrectly()
    {
        // Arrange
        var amount = 150.50m;
        var currency = Currency.Usd;

        // Act
        var money = new Money(amount, currency);

        // Assert
        money.Amount.ShouldBe(amount);
        money.Currency.ShouldBe(currency);
    }
}
