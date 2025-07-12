using System.ComponentModel.DataAnnotations;

namespace TravelAgency.ViewModels.Models.ReservationModels
{
    public class AddReservationViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Destination { get; set; } = null!;

        public string City { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public decimal Price { get; set; }

        public int Nights { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Reservation Date")]
        public DateTime ReservationDate { get; set; }
    }
}
