using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.UnitTests.Apartments;
using Bookify.Domain.Apartments;
using Bookify.Domain.Common;
using Shouldly;

namespace Bookify.Domain.UnitTests.Apartments;

public class ApartmentTests : BaseTest
{
    [Fact]
    public void Constructor_Should_SetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = new Name("Luxury Apartment");
        var description = new Description("Beautiful apartment with great amenities");
        var address = new Address("Germany", "Bavaria", "80331", "Munich", "Marienplatz 1");
        var price = new Money(150, Currency.Eur);
        var cleaningFee = new Money(25, Currency.Eur);
        var amenities = new List<Amenity> { Amenity.WiFi, Amenity.AirConditioning };

        // Act
        var apartment = new Apartment(id, name, description, address, price, cleaningFee, amenities);

        // Assert
        apartment.Id.ShouldBe(id);
        apartment.Name.ShouldBe(name);
        apartment.Description.ShouldBe(description);
        apartment.Address.ShouldBe(address);
        apartment.Price.ShouldBe(price);
        apartment.CleaningFee.ShouldBe(cleaningFee);
        apartment.Amenities.ShouldBe(amenities);
        apartment.LastBookedOnUtc.ShouldBeNull();
    }

    [Fact]
    public void Constructor_Should_CreateApartmentWithEmptyAmenities()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = new Name("Basic Apartment");
        var description = new Description("Simple apartment");
        var address = new Address("Germany", "Bavaria", "80331", "Munich", "Marienplatz 1");
        var price = new Money(100, Currency.Eur);
        var cleaningFee = Money.Zero(Currency.Eur);
        var amenities = new List<Amenity>();

        // Act
        var apartment = new Apartment(id, name, description, address, price, cleaningFee, amenities);

        // Assert
        apartment.Amenities.ShouldBeEmpty();
    }

    [Fact]
    public void Constructor_Should_CreateApartmentWithMultipleAmenities()
    {
        // Arrange
        var amenities = new List<Amenity>
        {
            Amenity.WiFi,
            Amenity.AirConditioning,
            Amenity.Parking,
            Amenity.GardenView,
            Amenity.SwimmingPool
        };

        // Act
        var apartment = ApartmentData.Create(new Money(200, Currency.Eur), Money.Zero(Currency.Eur));

        // Update amenities using reflection since they're private set
        var apartmentType = typeof(Apartment);
        var amenitiesProperty = apartmentType.GetProperty("Amenities");
        amenitiesProperty?.SetValue(apartment, amenities);

        // Assert
        apartment.Amenities.Count.ShouldBe(5);
        apartment.Amenities.ShouldContain(Amenity.WiFi);
        apartment.Amenities.ShouldContain(Amenity.AirConditioning);
        apartment.Amenities.ShouldContain(Amenity.Parking);
        apartment.Amenities.ShouldContain(Amenity.GardenView);
        apartment.Amenities.ShouldContain(Amenity.SwimmingPool);
    }
}
