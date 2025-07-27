namespace TravelAgency.ViewModels.Models.LandmarkModels
{
    public class GetAllLandmarksViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Destination { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public int FavoritesCount { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsDeleted { get; set; }
    }
}
