using System.ComponentModel.DataAnnotations;
using static TravelAgency.GCommon.ValidationConstants.Destination;
using static TravelAgency.GCommon.ValidationMessages.Destination;

namespace TravelAgency.ViewModels.Models.DestinationModels
{
    public class DestinationEditViewModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = NameIsRequerd)]
        [MinLength(MinLenghtCountryName, ErrorMessage = NameMinLenght)]
        [MaxLength(MaxLenghtCountryName, ErrorMessage = NameMaxLenght)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = DescriptionIsRequerd)]
        [MinLength(MinLenghtDescription, ErrorMessage = DescriptionMinLenght)]
        [MaxLength(MaxLenghtDescription, ErrorMessage = DescriptionMaxLenght)]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
