using Bookify.Domain.Reviews;

namespace Bookify.Domain.UnitTests.Reviews;

internal static class ReviewData
{
    public static readonly Comment Comment = new("Great apartment with excellent amenities!");
    public static readonly Comment EmptyComment = new("");
    
    public static Rating CreateValidRating(int value = 5) => Rating.Create(value).Value;
}
