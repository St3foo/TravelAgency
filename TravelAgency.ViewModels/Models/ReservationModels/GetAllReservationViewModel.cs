namespace TravelAgency.ViewModels.Models.ReservationModels
{
    public class GetAllReservationViewModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = null!;

        public Guid HotelId { get; set; }

        public string HotelName { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string Destination { get; set; } = null!;

        public string? ImageUrl { get; set; } = null!;

        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
