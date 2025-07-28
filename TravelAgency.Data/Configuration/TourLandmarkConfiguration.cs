using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Data.Models;
using static TravelAgency.GCommon.ValidationConstants;

namespace TravelAgency.Data.Configuration
{
    public class TourLandmarkConfiguration : IEntityTypeConfiguration<TourLandmark>
    {
        public void Configure(EntityTypeBuilder<TourLandmark> entity)
        {
            entity
                .HasKey(tl => new { tl.TourId , tl.LandmarkId});

            entity
                .HasOne(tl => tl.Tour)
                .WithMany(t => t.TourLandmarks)
                .HasForeignKey(tl => tl.TourId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(tl => tl.Landmark)
                .WithMany(l => l.TourLandmarks)
                .HasForeignKey(tl => tl.LandmarkId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasQueryFilter(tl => tl.Tour.IsDeleted == false && tl.Landmark.IsDeleted == false);

            entity
                .HasData(
                new TourLandmark 
                {
                    LandmarkId = Guid.Parse("5a883558-99eb-4c1a-8250-fcd2af1049bf"),
                    TourId = Guid.Parse("0f6cd9ef-202c-4b9d-8990-845cd0e7a708"),
                },
                new TourLandmark 
                {
                    LandmarkId = Guid.Parse("26d2d910-86f5-419a-a88a-a5b57206a72b"),
                    TourId = Guid.Parse("0f6cd9ef-202c-4b9d-8990-845cd0e7a708")
                },
                new TourLandmark
                {
                    LandmarkId = Guid.Parse("e896a347-9756-43b4-9791-dca8c5aeb84b") , 
                    TourId = Guid.Parse("2e01bf71-70b4-45ff-95a6-e1270ed10ad9")
                }, 
                new TourLandmark
                {
                    LandmarkId = Guid.Parse("e1264931-2355-4271-a612-398ca2f84b15"),
                    TourId = Guid.Parse("2e01bf71-70b4-45ff-95a6-e1270ed10ad9")
                }, 
                new TourLandmark
                {
                    LandmarkId = Guid.Parse("0a5bd069-b91b-49f7-acfe-2f0df5985028"),
                    TourId = Guid.Parse("ec31fe73-7caf-43a0-a1a9-7d09d335ffa6")
                },
                new TourLandmark
                {
                    LandmarkId = Guid.Parse("6824566a-9f3d-406e-b74a-f49f8aab1196"),
                    TourId = Guid.Parse("ec31fe73-7caf-43a0-a1a9-7d09d335ffa6")
                },
                new TourLandmark
                {
                    LandmarkId = Guid.Parse("847d8970-36dd-40da-af65-6f0d19162eb7"),
                    TourId = Guid.Parse("bb776480-2b10-4eff-9995-ebfe574ec833")
                },
                new TourLandmark
                {
                    LandmarkId = Guid.Parse("c61d4179-3893-43f4-bdbd-0df0d5d1a777"),
                    TourId = Guid.Parse("bb776480-2b10-4eff-9995-ebfe574ec833")
                });
        }
    }
}
