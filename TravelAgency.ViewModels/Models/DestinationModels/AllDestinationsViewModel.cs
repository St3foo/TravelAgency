using System.ComponentModel;

namespace TravelAgency.ViewModels.Models.DestinationModels
{
    public class AllDestinationsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
