using Bookify.Application.UnitTests.Apartments;
using Bookify.Application.UnitTests.Users;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Common;

namespace Bookify.Application.UnitTests.Bookings;

internal static class BookingData
{
    public static Booking Create(BookingStatus status = BookingStatus.Reserved)
    {
        var apartment = ApartmentData.Create();
        var user = UserData.Create();
        var duration = DateRange.Create(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
        var pricingService = new PricingService();

        var booking = Booking.Reserve(
            apartment,
            user.Id,
            duration,
            DateTime.UtcNow,
            pricingService);

        if (status == BookingStatus.Confirmed)
        {
            booking.Confirm(DateTime.UtcNow);
        }
        else if (status == BookingStatus.Completed)
        {
            booking.Confirm(DateTime.UtcNow);
            booking.Complete(DateTime.UtcNow);
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

        return booking;
    }
}
