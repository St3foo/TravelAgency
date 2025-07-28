using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Data.Models;
using static TravelAgency.GCommon.ValidationConstants.Tour;

namespace TravelAgency.Data.Configuration
{
    public class TourConfiguration : IEntityTypeConfiguration<Tour>
    {
        public void Configure(EntityTypeBuilder<Tour> entity)
        {
            entity
                .HasKey(t => t.Id);

            entity
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(MaxLenghtTourName);

            entity
                .Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(MaxLenghtDescription);

            entity
                .Property(t => t.IsDeleted)
                .HasDefaultValue(false);

            entity
                .Property(t => t.Price)
                .HasPrecision(18, 2);

            entity
                .HasOne(t => t.Destination)
                .WithMany(d => d.Tours)
                .HasForeignKey(t => t.DestinationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(t => t.Hotel)
                .WithMany(h => h.Tours)
                .HasForeignKey(t => t.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasQueryFilter(t => t.IsDeleted == false);

            entity
                .HasData(new Tour 
                {
                    Id = Guid.Parse("0f6cd9ef-202c-4b9d-8990-845cd0e7a708"),
                    Name = "Explore Japan",
                    ImageUrl = "https://www.smartmeetings.com/wp-content/uploads/2018/03/japan-1.jpg",
                    Description = "Day one visit : Fushimi Inari Taisha. Day two and three free days for exploration. Day four visit : Island of Honshu. Day five leave.",
                    DestinationId = Guid.Parse("c304fab0-e8e7-4d06-9ffb-607c502be4e6"),
                    HotelId = Guid.Parse("f8e2efd0-0e54-4c6d-8582-3651fa46f7b3"),
                    DaysStay = 5,
                    Price = 1500,
                },
                new Tour
                {
                    Id = Guid.Parse("2e01bf71-70b4-45ff-95a6-e1270ed10ad9"),
                    Name = "Explore Egypt",
                    ImageUrl = "https://www.traveltalktours.com/wp-content/smush-webp/2023/02/Great-Sphinx-of-Giza.jpeg.webp",
                    Description = "Day one and two explore : Piramids of Giza. Day three and four free time. Day five and six explore : Valley of the Kings. Day seven leave.",
                    DestinationId = Guid.Parse("f2af18bd-0c37-45dd-b545-3ff2beea2473"),
                    HotelId = Guid.Parse("d857605f-1a16-4539-afaf-5e42f604413b"),
                    DaysStay = 7,
                    Price = 1700,
                },
                new Tour
                {
                    Id = Guid.Parse("ec31fe73-7caf-43a0-a1a9-7d09d335ffa6"),
                    Name = "Explore Italy",
                    ImageUrl = "https://imgcld.yatra.com/ytimages/image/upload/t_holidays_srplist_tablet_hc/v5879035672/Holidays/Greenland/Venice_ycTyfo.jpg",
                    Description = "Day one explore : Colloseum , Day two and three free days. Day four explore : Leaning Tower of Pisa. Day five leave.",
                    DestinationId = Guid.Parse("7060a6cc-6942-4346-b2c0-0011337cbaff"),
                    HotelId = Guid.Parse("e27408c7-41dc-4504-b9c9-79f1cfe92f83"),
                    DaysStay = 5,
                    Price = 1400,
                },
                new Tour
                {
                    Id = Guid.Parse("bb776480-2b10-4eff-9995-ebfe574ec833"),
                    Name = "Explore United States of America",
                    ImageUrl = "https://miro.medium.com/v2/resize:fit:800/1*93v0YEC9A7Q8-8PxPwrstA.jpeg",
                    Description = "Day one explore : Niagara Falls. Day two free day. Day three explore : Statue of Liberty. Day four free day. Day five leave.",
                    DestinationId = Guid.Parse("bba3d9da-4843-448e-b0f0-ba9c60e91b08"),
                    HotelId = Guid.Parse("873c1c6c-c0aa-4a3c-b7fc-fc93cb31254b"),
                    DaysStay = 5,
                    Price = 2000,
                });
        }
    }
}
