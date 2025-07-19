using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Data.Models;

namespace TravelAgency.Data.Configuration
{
    public class UserHotelsConfiguration : IEntityTypeConfiguration<UserHotels>
    {
        public void Configure(EntityTypeBuilder<UserHotels> entity)
        {
            entity
                .HasKey(uh => uh.Id);

            entity
                .Property(uh => uh.UserId)
                .IsRequired();

            entity
                .HasOne(uh => uh.User)
                .WithMany()
                .HasForeignKey(uh => uh.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(uh => uh.Hotel)
                .WithMany(h => h.UsersHotels)
                .HasForeignKey(uh => uh.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .Property(uh => uh.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity
                .HasQueryFilter(uh => uh.Hotel.IsDeleted == false);
        }
    }
}
