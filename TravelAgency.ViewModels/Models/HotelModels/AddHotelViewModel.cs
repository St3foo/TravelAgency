using System.ComponentModel.DataAnnotations;
using TravelAgency.ViewModels.Models.DestinationModels;
using static TravelAgency.GCommon.ValidationConstants.Hotel;

namespace TravelAgency.ViewModels.Models.HotelModels
{
    public class AddHotelViewModel
    {
        [Required]
        [MinLength(MinLenghtHotelName)]
        [MaxLength(MaxLenghtHotelName)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(MinLenghtDescription)]
        [MaxLength(MaxLenghtDescription)]
        public string Description { get; set; } = null!;

        [Required]
        [MinLength(MinLenghtCityName)]
        [MaxLength(MaxLenghtCityName)]
        public string CityName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        [Required]
        public string DestinationId { get; set; } = null!;

        public IEnumerable<AllDestinationsViewModel>? Destinations { get; set; }

        public decimal Price { get; set; }

        public int Nights { get; set; }
    }
}
