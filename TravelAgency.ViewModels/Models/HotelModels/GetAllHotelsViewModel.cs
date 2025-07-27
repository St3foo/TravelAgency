namespace TravelAgency.ViewModels.Models.HotelModels
{
    public class GetAllHotelsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Destination { get; set; } = null!;

        public string City { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public int Nights { get; set; }

        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }
    }
}
