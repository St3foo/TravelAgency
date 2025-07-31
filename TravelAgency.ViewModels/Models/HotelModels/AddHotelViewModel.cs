using System.ComponentModel.DataAnnotations;
using TravelAgency.ViewModels.Models.DestinationModels;
using static TravelAgency.GCommon.ValidationConstants.Hotel;
using static TravelAgency.GCommon.ValidationMessages.Hotel;

namespace TravelAgency.ViewModels.Models.HotelModels
{
    public class AddHotelViewModel
    {
        [Required(ErrorMessage = HotelNameIsRequerd)]
        [MinLength(MinLenghtHotelName, ErrorMessage = HotelNameMinLenghtRequired)]
        [MaxLength(MaxLenghtHotelName, ErrorMessage = HotelNameMaxLenghtRequired)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = DescriptionIsRequerd)]
        [MinLength(MinLenghtDescription, ErrorMessage = DescriptionMinLenghtRequired)]
        [MaxLength(MaxLenghtDescription, ErrorMessage = DescriptionMaxLenghtRequired)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = CityNameIsRequerd)]
        [MinLength(MinLenghtCityName, ErrorMessage = CityNameMinLenghtRequired)]
        [MaxLength(MaxLenghtCityName, ErrorMessage = CityNameMaxLenghtRequired)]
        public string CityName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = DestinationIdIsRequered)]
        public string DestinationId { get; set; } = null!;

        public IEnumerable<AllDestinationsViewModel>? Destinations { get; set; }

        public decimal Price { get; set; }

        public int Nights { get; set; }
    }
}
