
namespace Bookify.Application.Abstractions.Clock;

// makes it easier to test and mock the system clock
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}