namespace TravelAgency.ViewModels.Models.TourModels
{
    public class TourDetailsViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string HotelName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string DestinationName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public decimal Price { get; set; }

        public int Nights { get; set; }

        public IEnumerable<GetLandmarksForToursViewModel> Landmarks { get; set; } = null!;
    }
}
