namespace TravelAgency.ViewModels.Models.TourModels
{
    public class GetAllToursViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Destination { get; set; } = null!;

        public string HotelName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public int Nights { get; set; }

        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }
    }
}
