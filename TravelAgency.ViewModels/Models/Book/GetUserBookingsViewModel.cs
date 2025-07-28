namespace TravelAgency.ViewModels.Models.Book
{
    public class GetUserBookingsViewModel
    {
        public Guid TourId { get; set; }

        public string Name { get; set; } = null!;

        public string HotelName { get; set; } = null!;

        public string DestinationName { get; set; } = null!;

        public string? ImageUrl { get; set; } = null!;

        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
