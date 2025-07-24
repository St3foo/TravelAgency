namespace TravelAgency.Data.Models
{
    public class Hotel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string CityName { get; set; } = null!;

        public string HotelName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public int DaysStay { get; set; }

        public bool IsDeleted { get; set; }

        public Guid DestinationId { get; set; }

        public virtual Destination Destination { get; set; } = null!;

        public virtual ICollection<UserHotel> UsersHotels { get; set; } = new HashSet<UserHotel>();
    }
}
