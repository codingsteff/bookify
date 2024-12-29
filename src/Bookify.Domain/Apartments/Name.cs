namespace Bookify.Domain.Apartments;

// Value object better than string, to ensure: validation (empty string, a name with only one character), immutability, equality, self-documenting
public record Name(string Value);