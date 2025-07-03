using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Data.Models;
using static TravelAgency.GCommon.ValidationConstants.Destination;

namespace TravelAgency.Data.Configuration
{
    public class DestinationConfiguration : IEntityTypeConfiguration<Destination>
    {
        public void Configure(EntityTypeBuilder<Destination> entity)
        {
            entity
                .HasKey(d => d.Id);

            entity
                .Property(d => d.CountryName)
                .IsRequired()
                .HasMaxLength(MaxLenghtCountryName);

            entity
                .Property(d => d.Description)
                .IsRequired()
                .HasMaxLength(MaxLenghtDescription);

            entity
                .Property(d => d.ImageUrl)
                .IsRequired(false);

            entity
                .Property(d => d.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(d => d.IsDeleted == false);
        }
    }
}
