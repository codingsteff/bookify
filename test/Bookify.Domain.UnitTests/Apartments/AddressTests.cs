using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.Apartments;
using Shouldly;

namespace Bookify.Domain.UnitTests.Apartments;

public class AddressTests : BaseTest
{
    [Fact]
    public void Constructor_Should_SetPropertiesCorrectly()
    {
        // Arrange
        var country = "Germany";
        var state = "Bavaria";
        var zipCode = "80331";
        var city = "Munich";
        var street = "Marienplatz 1";

        // Act
        var address = new Address(country, state, zipCode, city, street);

        // Assert
        address.Country.ShouldBe(country);
        address.State.ShouldBe(state);
        address.ZipCode.ShouldBe(zipCode);
        address.City.ShouldBe(city);
        address.Street.ShouldBe(street);
    }

    [Fact]
    public void Equals_Should_ReturnTrue_WhenAllPropertiesAreEqual()
    {
        // Arrange
        var address1 = new Address("Germany", "Bavaria", "80331", "Munich", "Marienplatz 1");
        var address2 = new Address("Germany", "Bavaria", "80331", "Munich", "Marienplatz 1");

        // Act & Assert
        address1.ShouldBe(address2);
    }

    [Fact]
    public void Equals_Should_ReturnFalse_WhenPropertiesAreDifferent()
    {
        // Arrange
        var address1 = new Address("Germany", "Bavaria", "80331", "Munich", "Marienplatz 1");
        var address2 = new Address("Germany", "Bavaria", "80331", "Munich", "Marienplatz 2");

        // Act & Assert
        address1.ShouldNotBe(address2);
    }

    [Fact]
    public void GetHashCode_Should_ReturnSameValue_WhenAddressesAreEqual()
    {
        // Arrange
        var address1 = new Address("Germany", "Bavaria", "80331", "Munich", "Marienplatz 1");
        var address2 = new Address("Germany", "Bavaria", "80331", "Munich", "Marienplatz 1");

        // Act & Assert
        address1.GetHashCode().ShouldBe(address2.GetHashCode());
    }
}
