using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Data.Models;

namespace TravelAgency.Data.Configuration
{
    public class UserLandmarksConfiguration : IEntityTypeConfiguration<UserLandmark>
    {
        public void Configure(EntityTypeBuilder<UserLandmark> entity)
        {
            entity
                .HasKey(ul => new { ul.UserId, ul.LandmarkId });

            entity
                .Property(ul => ul.UserId)
                .IsRequired();

            entity
                .HasOne(ul => ul.User)
                .WithMany()
                .HasForeignKey(ul => ul.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(ul => ul.Landmark)
                .WithMany(l => l.UserLandmarks)
                .HasForeignKey(ul => ul.LandmarkId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasQueryFilter(ul => ul.Landmark.IsDeleted == false);
        }
    }
}
