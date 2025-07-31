namespace Bookify.Domain.Users;

public record Email
{
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email cannot be empty.", nameof(value));
        }

        if (!IsValid(value))
        {
            throw new ArgumentException("Invalid email format.", nameof(value));
        }

        Value = value;
    }

    public string Value { get; }

    public static bool IsValid(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    
}