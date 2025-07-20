using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.Bookings;
using Shouldly;
using Bookify.Domain.UnitTests.Apartments;
using Bookify.Domain.Shared;
using Bookify.Domain.UnitTests.Users;
using Bookify.Domain.Users;
using Bookify.Domain.Bookings.Events;

namespace Bookify.Domain.UnitTests.Bookings;

public class BookingTests : BaseTest
{
    [Fact]
    public void Reserve_Should_RaiseBookingReservedDomainEvent()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(100, Currency.Eur);
        var apartment = ApartmentData.Create(price);
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var utcNow = DateTime.UtcNow;
        var pricingService = new PricingService();

        // Act
        var booking = Booking.Reserve(apartment, user.Id, duration, utcNow, pricingService);

        // Assert
        var domainEvent = AssertDomainEventWasPublished<BookingReservedDomainEvent>(booking);
        domainEvent.BookingId.ShouldBe(booking.Id);
    }

    [Fact]
    public void Reserve_Should_SetPropertiesCorrectly()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(100, Currency.Eur);
        var apartment = ApartmentData.Create(price);
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var utcNow = DateTime.UtcNow;
        var pricingService = new PricingService();

        // Act
        var booking = Booking.Reserve(apartment, user.Id, duration, utcNow, pricingService);

        // Assert
        booking.ApartmentId.ShouldBe(apartment.Id);
        booking.UserId.ShouldBe(user.Id);
        booking.Duration.ShouldBe(duration);
        booking.Status.ShouldBe(BookingStatus.Reserved);
        booking.CreatedOnUtc.ShouldBe(utcNow);
        booking.PriceForPeriod.Amount.ShouldBe(900); // 100 * 9 days
        booking.TotalPrice.Amount.ShouldBe(900);
    }

    [Fact]
    public void Reserve_Should_UpdateApartmentLastBookedOnUtc()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(100, Currency.Eur);
        var apartment = ApartmentData.Create(price);
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var utcNow = DateTime.UtcNow;
        var pricingService = new PricingService();

        // Act
        Booking.Reserve(apartment, user.Id, duration, utcNow, pricingService);

        // Assert
        apartment.LastBookedOnUtc.ShouldBe(utcNow);
    }

    [Fact]
    public void Confirm_Should_ReturnSuccessResult_WhenBookingIsReserved()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(100, Currency.Eur);
        var apartment = ApartmentData.Create(price);
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, duration, DateTime.UtcNow, pricingService);
        var confirmationTime = DateTime.UtcNow.AddHours(1);

        // Act
        var result = booking.Confirm(confirmationTime);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        booking.Status.ShouldBe(BookingStatus.Confirmed);
        booking.ConfirmedOnUtc.ShouldBe(confirmationTime);
    }

    [Fact]
    public void Confirm_Should_RaiseBookingConfirmedDomainEvent_WhenBookingIsReserved()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(100, Currency.Eur);
        var apartment = ApartmentData.Create(price);
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, duration, DateTime.UtcNow, pricingService);

        // Act
        booking.Confirm(DateTime.UtcNow.AddHours(1));

        // Assert
        var domainEvent = AssertDomainEventWasPublished<BookingConfirmedDomainEvent>(booking);
        domainEvent.BookingId.ShouldBe(booking.Id);
    }

    [Fact]
    public void Reject_Should_ReturnSuccessResult_WhenBookingIsReserved()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(100, Currency.Eur);
        var apartment = ApartmentData.Create(price);
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, duration, DateTime.UtcNow, pricingService);
        var rejectionTime = DateTime.UtcNow.AddHours(1);

        // Act
        var result = booking.Reject(rejectionTime);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        booking.Status.ShouldBe(BookingStatus.Rejected);
        booking.RejectedOnUtc.ShouldBe(rejectionTime);
    }

    [Fact]
    public void Reject_Should_RaiseBookingRejectedDomainEvent_WhenBookingIsReserved()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(100, Currency.Eur);
        var apartment = ApartmentData.Create(price);
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, duration, DateTime.UtcNow, pricingService);

        // Act
        booking.Reject(DateTime.UtcNow.AddHours(1));

        // Assert
        var domainEvent = AssertDomainEventWasPublished<BookingRejectedDomainEvent>(booking);
        domainEvent.BookingId.ShouldBe(booking.Id);
    }

    [Fact]
    public void Complete_Should_ReturnSuccessResult_WhenBookingIsConfirmed()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(100, Currency.Eur);
        var apartment = ApartmentData.Create(price);
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, duration, DateTime.UtcNow, pricingService);
        booking.Confirm(DateTime.UtcNow.AddHours(1));
        var completionTime = DateTime.UtcNow.AddDays(10);

        // Act
        var result = booking.Complete(completionTime);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        booking.Status.ShouldBe(BookingStatus.Completed);
        booking.CompletedOnUtc.ShouldBe(completionTime);
    }

    [Fact]
    public void Complete_Should_RaiseBookingCompletedDomainEvent_WhenBookingIsConfirmed()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(100, Currency.Eur);
        var apartment = ApartmentData.Create(price);
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, duration, DateTime.UtcNow, pricingService);
        booking.Confirm(DateTime.UtcNow.AddHours(1));

        // Act
        booking.Complete(DateTime.UtcNow.AddDays(10));

        // Assert
        var domainEvent = AssertDomainEventWasPublished<BookingCompletedDomainEvent>(booking);
        domainEvent.BookingId.ShouldBe(booking.Id);
    }

    [Theory]
    [InlineData(BookingStatus.Reserved)]
    [InlineData(BookingStatus.Rejected)]
    [InlineData(BookingStatus.Completed)]
    [InlineData(BookingStatus.Cancelled)]
    public void Complete_Should_ReturnFailureResult_WhenBookingIsNotConfirmed(BookingStatus initialStatus)
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(100, Currency.Eur);
        var apartment = ApartmentData.Create(price);
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, duration, DateTime.UtcNow, pricingService);

        // Set initial status
        if (initialStatus == BookingStatus.Rejected)
        {
            booking.Reject(DateTime.UtcNow);
        }

        // Act
        var result = booking.Complete(DateTime.UtcNow.AddDays(10));

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(BookingErrors.NotConfirmed);
    }

    [Fact]
    public void Cancel_Should_ReturnSuccessResult_WhenBookingIsConfirmedAndNotStarted()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(100, Currency.Eur);
        var apartment = ApartmentData.Create(price);
        var futureStartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
        var duration = DateRange.Create(futureStartDate, futureStartDate.AddDays(5));
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, duration, DateTime.UtcNow, pricingService);
        booking.Confirm(DateTime.UtcNow.AddHours(1));
        var cancellationTime = DateTime.UtcNow.AddDays(1);

        // Act
        var result = booking.Cancel(cancellationTime);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        booking.Status.ShouldBe(BookingStatus.Cancelled);
        booking.CancelledOnUtc.ShouldBe(cancellationTime);
    }

    [Fact]
    public void Cancel_Should_RaiseBookingCancelledDomainEvent_WhenBookingIsConfirmedAndNotStarted()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(100, Currency.Eur);
        var apartment = ApartmentData.Create(price);
        var futureStartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
        var duration = DateRange.Create(futureStartDate, futureStartDate.AddDays(5));
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, duration, DateTime.UtcNow, pricingService);
        booking.Confirm(DateTime.UtcNow.AddHours(1));

        // Act
        booking.Cancel(DateTime.UtcNow.AddDays(1));

        // Assert
        var domainEvent = AssertDomainEventWasPublished<BookingCancelledDomainEvent>(booking);
        domainEvent.BookingId.ShouldBe(booking.Id);
    }

    [Fact]
    public void Cancel_Should_ReturnFailureResult_WhenBookingHasAlreadyStarted()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(100, Currency.Eur);
        var apartment = ApartmentData.Create(price);
        var pastStartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2));
        var duration = DateRange.Create(pastStartDate, pastStartDate.AddDays(5));
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, duration, DateTime.UtcNow.AddDays(-5), pricingService);
        booking.Confirm(DateTime.UtcNow.AddDays(-4));

        // Act
        var result = booking.Cancel(DateTime.UtcNow);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(BookingErrors.AlreadyStarted);
    }

    [Theory]
    [InlineData(BookingStatus.Reserved)]
    [InlineData(BookingStatus.Rejected)]
    [InlineData(BookingStatus.Completed)]
    [InlineData(BookingStatus.Cancelled)]
    public void Cancel_Should_ReturnFailureResult_WhenBookingIsNotConfirmed(BookingStatus initialStatus)
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(100, Currency.Eur);
        var apartment = ApartmentData.Create(price);
        var futureStartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
        var duration = DateRange.Create(futureStartDate, futureStartDate.AddDays(5));
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, duration, DateTime.UtcNow, pricingService);

        // Set initial status
        if (initialStatus == BookingStatus.Rejected)
        {
            booking.Reject(DateTime.UtcNow);
        }

        // Act
        var result = booking.Cancel(DateTime.UtcNow.AddDays(1));

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(BookingErrors.NotConfirmed);
    }

}