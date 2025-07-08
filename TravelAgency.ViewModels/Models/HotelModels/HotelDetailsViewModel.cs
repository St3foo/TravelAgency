namespace TravelAgency.ViewModels.Models.HotelModels
{
    public class HotelDetailsViewModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Destination { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public decimal Price { get; set; }

        public int Nights { get; set; }
    }
}
