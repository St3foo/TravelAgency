using System.ComponentModel.DataAnnotations;
using TravelAgency.ViewModels.Models.DestinationModels;
using static TravelAgency.GCommon.ValidationConstants.Landmark;
using static TravelAgency.GCommon.ValidationMessages.Landmark;

namespace TravelAgency.ViewModels.Models.LandmarkModels
{
    public class AddLandmarkViewModel
    {
        [Required(ErrorMessage = NameIsRequerd)]
        [MinLength(MinLenghtName, ErrorMessage = NameMinLenghtRequired)]
        [MaxLength(MaxLenghtName, ErrorMessage = NameMaxLenghtRequired)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = LocationNameIsRequerd)]
        [MinLength(MinLenghtLocation, ErrorMessage = LocationNameMinLenghtRequired)]
        [MaxLength(MaxLenghtLocation, ErrorMessage = LocationNameMaxLenghtRequired)]
        public string Location { get; set; } = null!;

        [Required(ErrorMessage = DescriptionIsRequerd)]
        [MinLength(MinLenghtDescription, ErrorMessage = DescriptionMinLenghtRequired)]
        [MaxLength(MaxLenghtDescription, ErrorMessage = DescriptionMaxLenghtRequired)]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = DestinationIdIsRequered)]
        public string DestinationId { get; set; } = null!;

        public IEnumerable<AllDestinationsViewModel>? Destinations { get; set; }
    }
}
