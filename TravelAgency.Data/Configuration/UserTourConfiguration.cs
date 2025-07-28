using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Data.Models;

namespace TravelAgency.Data.Configuration
{
    public class UserTourConfiguration : IEntityTypeConfiguration<UserTour>
    {
        public void Configure(EntityTypeBuilder<UserTour> entity)
        {
            entity
                .HasKey(ut => ut.Id);

            entity
                .Property(ut => ut.UserId)
                .IsRequired();

            entity
                .HasOne(ut => ut.User)
                .WithMany()
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(ut => ut.Tour)
                .WithMany(t => t.UserTours)
                .HasForeignKey(ut => ut.TourId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .Property(ut => ut.IsActive)
                .HasDefaultValue(true);

            entity
                .HasQueryFilter(ut => ut.Tour.IsDeleted == false);
        }
    }
}
