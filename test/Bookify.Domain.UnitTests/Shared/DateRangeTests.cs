using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.Shared;
using Shouldly;

namespace Bookify.Domain.UnitTests.Shared;

public class DateRangeTests : BaseTest
{
    [Fact]
    public void Create_Should_CreateDateRange_WhenStartIsBeforeEnd()
    {
        // Arrange
        var start = new DateOnly(2025, 1, 1);
        var end = new DateOnly(2025, 1, 10);

        // Act
        var dateRange = DateRange.Create(start, end);

        // Assert
        dateRange.Start.ShouldBe(start);
        dateRange.End.ShouldBe(end);
    }

    [Fact]
    public void Create_Should_CreateDateRange_WhenStartEqualsEnd()
    {
        // Arrange
        var date = new DateOnly(2025, 1, 1);

        // Act
        var dateRange = DateRange.Create(date, date);

        // Assert
        dateRange.Start.ShouldBe(date);
        dateRange.End.ShouldBe(date);
    }

    [Fact]
    public void Create_Should_ThrowApplicationException_WhenEndPrecedesStart()
    {
        // Arrange
        var start = new DateOnly(2025, 1, 10);
        var end = new DateOnly(2025, 1, 1);

        // Act & Assert
        Should.Throw<ApplicationException>(() => DateRange.Create(start, end))
            .Message.ShouldBe("End date precedes start date");
    }

    [Fact]
    public void LengthInDays_Should_ReturnCorrectValue_WhenDateRangeIsValid()
    {
        // Arrange
        var start = new DateOnly(2025, 1, 1);
        var end = new DateOnly(2025, 1, 10);
        var dateRange = DateRange.Create(start, end);

        // Act
        var lengthInDays = dateRange.LengthInDays;

        // Assert
        lengthInDays.ShouldBe(9);
    }

    [Fact]
    public void LengthInDays_Should_ReturnZero_WhenStartEqualsEnd()
    {
        // Arrange
        var date = new DateOnly(2025, 1, 1);
        var dateRange = DateRange.Create(date, date);

        // Act
        var lengthInDays = dateRange.LengthInDays;

        // Assert
        lengthInDays.ShouldBe(0);
    }
}
