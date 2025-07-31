using System.ComponentModel.DataAnnotations;
using TravelAgency.ViewModels.Models.HotelModels;
using TravelAgency.ViewModels.Models.LandmarkModels;
using static TravelAgency.GCommon.ValidationConstants.Tour;
using static TravelAgency.GCommon.ValidationMessages.Tour;

namespace TravelAgency.ViewModels.Models.TourModels
{
    public class AddTourViewModel
    {
        [Required(ErrorMessage = NameIsRequerd)]
        [MinLength(MinLenghtTourName, ErrorMessage = NameMinLenghtRequired)]
        [MaxLength(MaxLenghtTourName, ErrorMessage = NameMaxLenghtRequired)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = DescriptionIsRequerd)]
        [MinLength(MinLenghtDescription, ErrorMessage = DescriptionMinLenghtRequired)]
        [MaxLength(MaxLenghtDescription, ErrorMessage = DescriptionMaxLenghtRequired)]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public Guid DestinationId { get; set; }

        public Guid HotelId { get; set; }

        public decimal Price { get; set; }

        public int DaysStay { get; set; }

        public Guid[]? Landmarks { get; set; }

        public IEnumerable<GetAllHotelsForAddTourViewModel>? AllHotels { get; set; } 

        public IEnumerable<GetLandmarksForToursViewModel>? AllLanadmarks { get; set; }
    }
}
