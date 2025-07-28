namespace TravelAgency.ViewModels.Models.TourModels
{
    public class GetLandmarksForToursViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
