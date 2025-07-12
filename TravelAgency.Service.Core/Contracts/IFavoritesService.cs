using TravelAgency.ViewModels.Models.FavoritesModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface IFavoritesService
    {
        Task<IEnumerable<GetAllFavoritesViewModel>> GetAllFavoritesLandmarksAsync(string? userId);

        Task AddToFavoritesAsync(string? userId, string? landmarkId);

        Task RemoveFromFavoritesAsync(string? userId, string? landmarkId);
    }
}
