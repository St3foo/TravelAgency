namespace TravelAgency.Data.Models
{
    public class Tour
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public bool IsDeleted { get; set; }

        public int DaysStay { get; set; }

        public decimal Price { get; set; }

        public Guid DestinationId { get; set; }

        public virtual Destination Destination { get; set; } = null!;

        public Guid HotelId { get; set; }

        public virtual Hotel Hotel { get; set; } = null!;

        public virtual ICollection<TourLandmark> TourLandmarks { get; set; } = new HashSet<TourLandmark>();

        public virtual ICollection<UserTour> UserTours { get; set; } = new HashSet<UserTour>();
    }
}
