using System.ComponentModel.DataAnnotations;
using static TravelAgency.GCommon.ValidationConstants.Destination;

namespace TravelAgency.ViewModels.Models.DestinationModels
{
    public class AddDestinationViewModel
    {
        [Required]
        [MinLength(MinLenghtCountryName)]
        [MaxLength(MaxLenghtCountryName)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(MinLenghtDescription)]
        [MaxLength(MaxLenghtDescription)]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
