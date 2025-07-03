using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Data.Models;
using static TravelAgency.GCommon.ValidationConstants.Hotel;

namespace TravelAgency.Data.Configuration
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> entity)
        {
            entity
                .HasKey(h => h.Id);

            entity
                .Property(h => h.CityName)
                .IsRequired()
                .HasMaxLength(MaxLenghtCityName);

            entity
                .Property(h => h.HotelName)
                .IsRequired()
                .HasMaxLength(MaxLenghtHotelName);

            entity
                .Property(h => h.ImageUrl)
                .IsRequired(false);

            entity
                .Property(h => h.Description)
                .IsRequired()
                .HasMaxLength(MaxLenghtDescription);

            entity
                .Property(h => h.Price)
                .HasPrecision(18, 2);

            entity
                .Property(h => h.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasOne(h => h.Destination)
                .WithMany(d => d.Hotels)
                .HasForeignKey(h => h.DestinationId);

            entity
                .HasQueryFilter(h => h.IsDeleted == false);
        }
    }
}
