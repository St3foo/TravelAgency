namespace TravelAgency.ViewModels.Models.LandmarkModels
{
    public class LandmarkDetailsViewModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Destination { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
