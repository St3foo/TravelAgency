using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Data.Models;
using static TravelAgency.GCommon.ValidationConstants.Landmark;

namespace TravelAgency.Data.Configuration
{
    public class LandmarkConfiguration : IEntityTypeConfiguration<Landmark>
    {
        public void Configure(EntityTypeBuilder<Landmark> entity)
        {
            entity
                .HasKey(l => l.Id);

            entity
                .Property(l => l.LocationName)
                .IsRequired()
                .HasMaxLength(MaxLenghtLocation);

            entity
                .Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(MaxLenghtName);

            entity
                .Property(l => l.Description)
                .IsRequired()
                .HasMaxLength(MaxLenghtDescription);

            entity
                .Property(l => l.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasOne(l => l.Destination)
                .WithMany(d => d.Landmarks)
                .HasForeignKey(l => l.DestinationId);

            entity
                .HasQueryFilter(l => l.IsDeleted == false);
        }
    }
}
