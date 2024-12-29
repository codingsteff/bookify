namespace Bookify.Domain.Apartments;

// Value object representing an address. record is ideal
public record Address(
    string Country,
    string State,
    string ZipCode,
    string City,
    string Street
);