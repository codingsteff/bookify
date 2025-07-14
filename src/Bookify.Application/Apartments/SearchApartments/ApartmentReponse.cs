namespace Bookify.Application.Apartments.SearchApartments;

public sealed class ApartmentResponse
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public decimal Price { get; init; }
    public string? Currency { get; init; }
    public required AddressResponse Address { get; set; }
}