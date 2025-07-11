namespace TravelAgency.ViewModels.Models.LandmarkModels
{
    public class DeleteLandmarkViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
