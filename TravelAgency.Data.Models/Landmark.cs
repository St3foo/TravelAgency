namespace TravelAgency.Data.Models
{
    public class Landmark
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string LocationName { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string Description { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public Guid DestinationId { get; set; }

        public virtual Destination Destination { get; set; } = null!;

        public virtual ICollection<UserLandmark> UserLandmarks { get; set; } = new HashSet<UserLandmark>();
    }
}
