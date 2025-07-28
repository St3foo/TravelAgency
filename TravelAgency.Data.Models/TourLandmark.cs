namespace TravelAgency.Data.Models
{
    public class TourLandmark
    {
        public Guid TourId { get; set; }

        public virtual Tour Tour { get; set; } = null!;

        public Guid LandmarkId { get; set; }

        public virtual Landmark Landmark { get; set; } = null!;
    }
}
