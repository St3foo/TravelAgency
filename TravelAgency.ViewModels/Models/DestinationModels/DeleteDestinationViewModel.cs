namespace TravelAgency.ViewModels.Models.DestinationModels
{
    public class DeleteDestinationViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
