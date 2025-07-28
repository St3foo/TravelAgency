using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data.Models;

namespace TravelAgency.Data
{
    public class TravelAgencyDbContext : IdentityDbContext
    {
        public TravelAgencyDbContext(DbContextOptions<TravelAgencyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Destination> Destinations { get; set; } = null!;

        public virtual DbSet<Hotel> Hotels { get; set; } = null!;

        public virtual DbSet<Landmark> Landmarks { get; set; } = null!;

        public virtual DbSet<Tour> Tours { get; set; } = null!;

        public virtual DbSet<UserHotel> UsersHotels { get; set; } = null!;

        public virtual DbSet<UserLandmark> UsersLandmarks { get; set; } = null!;

        public virtual DbSet<UserTour> UsersTours { get; set; } = null!;

        public virtual DbSet<TourLandmark> ToursLandmarks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder entity)
        {
            base.OnModelCreating(entity);

            entity
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
