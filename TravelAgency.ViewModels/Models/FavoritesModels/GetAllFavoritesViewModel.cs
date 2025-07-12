namespace TravelAgency.ViewModels.Models.FavoritesModels
{
    public class GetAllFavoritesViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string Destination { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
