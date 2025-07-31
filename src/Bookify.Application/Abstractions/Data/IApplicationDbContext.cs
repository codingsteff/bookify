using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews;
using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Apartment> Apartments { get; }

    DbSet<Booking> Bookings { get; }

    DbSet<Review> Reviews { get; }

    DbSet<User> Users { get; }
}
