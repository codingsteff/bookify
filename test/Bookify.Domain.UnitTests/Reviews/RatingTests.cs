using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.Reviews;
using Shouldly;

namespace Bookify.Domain.UnitTests.Reviews;

public class RatingTests : BaseTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Create_Should_ReturnSuccessResult_WhenRatingIsValid(int value)
    {
        // Act
        var result = Rating.Create(value);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Value.ShouldBe(value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(6)]
    [InlineData(10)]
    public void Create_Should_ReturnFailureResult_WhenRatingIsInvalid(int value)
    {
        // Act
        var result = Rating.Create(value);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(Rating.Invalid);
    }
}
