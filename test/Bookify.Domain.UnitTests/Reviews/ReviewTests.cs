using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.UnitTests.Apartments;
using Bookify.Domain.UnitTests.Users;
using Bookify.Domain.Reviews;
using Bookify.Domain.Reviews.Events;
using Bookify.Domain.Bookings;
using Bookify.Domain.Common;
using Shouldly;

namespace Bookify.Domain.UnitTests.Reviews;

public class ReviewTests : BaseTest
{
    [Fact]
    public void Create_Should_ReturnSuccessResult_WhenBookingIsCompleted()
    {
        // Arrange
        var user = Domain.Users.User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var apartment = ApartmentData.Create(new Money(100, Currency.Eur));
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, duration, DateTime.UtcNow, pricingService);
        
        // Complete the booking
        booking.Confirm(DateTime.UtcNow);
        booking.Complete(DateTime.UtcNow.AddDays(10));
        
        var rating = ReviewData.CreateValidRating();
        var comment = ReviewData.Comment;
        var createdOnUtc = DateTime.UtcNow;

        // Act
        var result = Review.Create(booking, rating, comment, createdOnUtc);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        var review = result.Value;
        review.ApartmentId.ShouldBe(booking.ApartmentId);
        review.BookingId.ShouldBe(booking.Id);
        review.UserId.ShouldBe(booking.UserId);
        review.Rating.ShouldBe(rating);
        review.Comment.ShouldBe(comment);
        review.CreatedOnUtc.ShouldBe(createdOnUtc);
    }

    [Fact]
    public void Create_Should_RaiseReviewCreatedDomainEvent_WhenBookingIsCompleted()
    {
        // Arrange
        var user = Domain.Users.User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var apartment = ApartmentData.Create(new Money(100, Currency.Eur));
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, duration, DateTime.UtcNow, pricingService);
        
        // Complete the booking
        booking.Confirm(DateTime.UtcNow);
        booking.Complete(DateTime.UtcNow.AddDays(10));
        
        var rating = ReviewData.CreateValidRating();
        var comment = ReviewData.Comment;

        // Act
        var result = Review.Create(booking, rating, comment, DateTime.UtcNow);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        var review = result.Value;
        var domainEvent = AssertDomainEventWasPublished<ReviewCreatedDomainEvent>(review);
        domainEvent.ReviewId.ShouldBe(review.Id);
    }

    [Theory]
    [InlineData(BookingStatus.Reserved)]
    [InlineData(BookingStatus.Confirmed)]
    [InlineData(BookingStatus.Rejected)]
    [InlineData(BookingStatus.Cancelled)]
    public void Create_Should_ReturnFailureResult_WhenBookingIsNotCompleted(BookingStatus status)
    {
        // Arrange
        var user = Domain.Users.User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var apartment = ApartmentData.Create(new Money(100, Currency.Eur));
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, duration, DateTime.UtcNow, pricingService);
        
        // Set the desired status (except Completed)
        if (status == BookingStatus.Confirmed)
        {
            booking.Confirm(DateTime.UtcNow);
        }
        else if (status == BookingStatus.Rejected)
        {
            booking.Reject(DateTime.UtcNow);
        }
        else if (status == BookingStatus.Cancelled)
        {
            booking.Confirm(DateTime.UtcNow);
            booking.Cancel(DateTime.UtcNow);
        }
        
        var rating = ReviewData.CreateValidRating();
        var comment = ReviewData.Comment;

        // Act
        var result = Review.Create(booking, rating, comment, DateTime.UtcNow);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(ReviewErrors.NotEligible);
    }
}
