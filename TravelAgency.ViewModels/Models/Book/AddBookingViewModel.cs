using System.ComponentModel.DataAnnotations;
using TravelAgency.ViewModels.Models.TourModels;

namespace TravelAgency.ViewModels.Models.Book
{
    public class AddBookingViewModel
    {
        public Guid Id { get; set; } 

        public string TourName { get; set; } = null!;

        public string HotelName { get; set; } = null!;

        public string DestinationName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public decimal Price { get; set; }

        public int Nights { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; }
    }
}
