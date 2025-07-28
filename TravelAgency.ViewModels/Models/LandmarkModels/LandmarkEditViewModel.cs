using System.ComponentModel.DataAnnotations;
using TravelAgency.ViewModels.Models.DestinationModels;
using static TravelAgency.GCommon.ValidationConstants.Landmark;
using static TravelAgency.GCommon.ValidationMessages.Landmark;

namespace TravelAgency.ViewModels.Models.LandmarkModels
{
    public class LandmarkEditViewModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = NameIsRequerd)]
        [MinLength(MinLenghtName, ErrorMessage = NameMinLenght)]
        [MaxLength(MaxLenghtName, ErrorMessage = NameMaxLenght)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = LocationNameIsRequerd)]
        [MinLength(MinLenghtLocation, ErrorMessage = LocationNameMinLenght)]
        [MaxLength(MaxLenghtLocation, ErrorMessage = LocationNameMaxLenght)]
        public string Location { get; set; } = null!;

        [Required(ErrorMessage = DescriptionIsRequerd)]
        [MinLength(MinLenghtDescription, ErrorMessage = DescriptionMinLenght)]
        [MaxLength(MaxLenghtDescription, ErrorMessage = DescriptionMaxLenght)]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = DestinationIdIsRequered)]
        public string DestinationId { get; set; } = null!;

        public IEnumerable<AllDestinationsViewModel>? Destinations { get; set; }
    }
}
