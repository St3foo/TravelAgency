namespace TravelAgency.ViewModels.Models.Book
{
    public class GetAllBookingsViewModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = null!;

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
