namespace TravelAgency.Data.Models
{
    public class Destination
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string CountryName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string Description { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public virtual ICollection<Hotel> Hotels { get; set; } = new HashSet<Hotel>();

        public virtual ICollection<Landmark> Landmarks { get; set; } = new HashSet<Landmark>();
    }
}
