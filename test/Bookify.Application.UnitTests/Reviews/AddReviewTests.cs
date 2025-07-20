using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Reviews.AddReview;
using Bookify.Application.UnitTests.Bookings;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews;
using NSubstitute;
using Shouldly;

namespace Bookify.Application.UnitTests.Reviews;

public class AddReviewTests
{
    private static readonly DateTime UtcNow = DateTime.UtcNow;
    private static readonly AddReviewCommand Command = new(
        Guid.NewGuid(),
        5,
        "Excellent apartment!");

    private readonly AddReviewCommandHandler _handler;
    private readonly IBookingRepository _bookingRepositoryMock;
    private readonly IReviewRepository _reviewRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IDateTimeProvider _dateTimeProviderMock;

    public AddReviewTests()
    {
        _bookingRepositoryMock = Substitute.For<IBookingRepository>();
        _reviewRepositoryMock = Substitute.For<IReviewRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
        _dateTimeProviderMock.UtcNow.Returns(UtcNow);

        _handler = new AddReviewCommandHandler(
            _bookingRepositoryMock,
            _reviewRepositoryMock,
            _unitOfWorkMock,
            _dateTimeProviderMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenBookingIsNull()
    {
        // Arrange
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, Arg.Any<CancellationToken>())
            .Returns((Booking?)null);

        // Act
        var result = await _handler.Handle(Command, TestContext.Current.CancellationToken);

        // Assert
        result.Error.ShouldBe(BookingErrors.NotFound);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    [InlineData(10)]
    public async Task Handle_Should_ReturnFailure_WhenRatingIsInvalid(int invalidRating)
    {
        // Arrange
        var booking = BookingData.Create(BookingStatus.Completed);
        var commandWithInvalidRating = new AddReviewCommand(
            Command.BookingId,
            invalidRating,
            Command.Comment);

        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, Arg.Any<CancellationToken>())
            .Returns(booking);

        // Act
        var result = await _handler.Handle(commandWithInvalidRating, TestContext.Current.CancellationToken);

        // Assert
        result.Error.ShouldBe(Rating.Invalid);
    }

    [Theory]
    [InlineData(BookingStatus.Reserved)]
    [InlineData(BookingStatus.Confirmed)]
    [InlineData(BookingStatus.Rejected)]
    [InlineData(BookingStatus.Cancelled)]
    public async Task Handle_Should_ReturnFailure_WhenBookingIsNotCompleted(BookingStatus status)
    {
        // Arrange
        var booking = BookingData.Create(status);

        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, Arg.Any<CancellationToken>())
            .Returns(booking);

        // Act
        var result = await _handler.Handle(Command, TestContext.Current.CancellationToken);

        // Assert
        result.Error.ShouldBe(ReviewErrors.NotEligible);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenBookingIsCompletedAndRatingIsValid()
    {
        // Arrange
        var booking = BookingData.Create(BookingStatus.Completed);

        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, Arg.Any<CancellationToken>())
            .Returns(booking);

        // Act
        var result = await _handler.Handle(Command, TestContext.Current.CancellationToken);

        // Assert
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Handle_Should_CallReviewRepository_WhenReviewIsCreated()
    {
        // Arrange
        var booking = BookingData.Create(BookingStatus.Completed);

        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, Arg.Any<CancellationToken>())
            .Returns(booking);

        // Act
        var result = await _handler.Handle(Command, TestContext.Current.CancellationToken);

        // Assert
        _reviewRepositoryMock.Received(1).Add(Arg.Is<Review>(r =>
            r.BookingId == booking.Id &&
            r.ApartmentId == booking.ApartmentId &&
            r.UserId == booking.UserId &&
            r.Rating.Value == Command.Rating &&
            r.Comment.Value == Command.Comment));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Handle_Should_CreateReviewWithCorrectRating_WhenRatingIsValid(int validRating)
    {
        // Arrange
        var booking = BookingData.Create(BookingStatus.Completed);
        var commandWithRating = new AddReviewCommand(
            Command.BookingId,
            validRating,
            Command.Comment);

        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, Arg.Any<CancellationToken>())
            .Returns(booking);

        // Act
        var result = await _handler.Handle(commandWithRating, TestContext.Current.CancellationToken);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        _reviewRepositoryMock.Received(1).Add(Arg.Is<Review>(r => r.Rating.Value == validRating));
    }

}
