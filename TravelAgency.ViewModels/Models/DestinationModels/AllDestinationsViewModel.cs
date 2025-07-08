using System.ComponentModel;

namespace TravelAgency.ViewModels.Models.DestinationModels
{
    public class AllDestinationsViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
