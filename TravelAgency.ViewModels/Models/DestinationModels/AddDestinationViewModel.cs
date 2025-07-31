using System.ComponentModel.DataAnnotations;
using static TravelAgency.GCommon.ValidationConstants.Destination;
using static TravelAgency.GCommon.ValidationMessages.Destination;

namespace TravelAgency.ViewModels.Models.DestinationModels
{
    public class AddDestinationViewModel
    {
        [Required(ErrorMessage = NameIsRequerd)]
        [MinLength(MinLenghtCountryName, ErrorMessage = NameMinLenghtRequired)]
        [MaxLength(MaxLenghtCountryName, ErrorMessage = NameMaxLenghtRequired)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = DescriptionIsRequerd)]
        [MinLength(MinLenghtDescription, ErrorMessage = DescriptionMinLenghtRequired)]
        [MaxLength(MaxLenghtDescription, ErrorMessage = DescriptionMaxLenghtRequired)]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
