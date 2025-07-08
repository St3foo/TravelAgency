namespace TravelAgency.ViewModels.Models.DestinationModels
{
    public class DestinationDetailViewModel
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string Description { get; set; } = null!;
    }
}
