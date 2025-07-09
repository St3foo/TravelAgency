using System.ComponentModel.DataAnnotations;
using TravelAgency.ViewModels.Models.DestinationModels;
using static TravelAgency.GCommon.ValidationConstants.Landmark;

namespace TravelAgency.ViewModels.Models.LandmarkModels
{
    public class AddLandmarkViewModel
    {
        [Required]
        [MinLength(MinLenghtName)]
        [MaxLength(MaxLenghtName)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(MinLenghtLocation)]
        [MaxLength(MaxLenghtLocation)]
        public string Location { get; set; } = null!;

        [Required]
        [MinLength(MinLenghtDescription)]
        [MaxLength(MaxLenghtDescription)]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }

        [Required]
        public string DestinationId { get; set; } = null!;

        public IEnumerable<AllDestinationsViewModel>? Destinations { get; set; }
    }
}
