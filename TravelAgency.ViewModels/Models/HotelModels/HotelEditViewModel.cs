using System.ComponentModel.DataAnnotations;
using TravelAgency.ViewModels.Models.DestinationModels;
using static TravelAgency.GCommon.ValidationConstants.Hotel;
using static TravelAgency.GCommon.ValidationMessages.Hotel;

namespace TravelAgency.ViewModels.Models.HotelModels
{
    public class HotelEditViewModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = HotelNameIsRequerd)]
        [MinLength(MinLenghtHotelName, ErrorMessage = HotelNameMinLenght)]
        [MaxLength(MaxLenghtHotelName, ErrorMessage = HotelNameMaxLenght)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = DescriptionIsRequerd)]
        [MinLength(MinLenghtDescription, ErrorMessage = DescriptionMinLenght)]
        [MaxLength(MaxLenghtDescription, ErrorMessage = DescriptionMaxLenght)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = CityNameIsRequerd)]
        [MinLength(MinLenghtCityName, ErrorMessage = CityNameMinLenght)]
        [MaxLength(MaxLenghtCityName, ErrorMessage = CityNameMaxLenght)]
        public string CityName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = DestinationIdIsRequered)]
        public string DestinationId { get; set; } = null!;

        public IEnumerable<AllDestinationsViewModel>? Destinations { get; set; }

        public decimal Price { get; set; }

        public int Nights { get; set; }
    }
}
