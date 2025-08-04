using TravelAgency.ViewModels.Models.LandmarkModels;
using TravelAgency.ViewModels.Models.TourModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface ILandmarkService
    {
        Task<IEnumerable<GetAllLandmarksViewModel>> GetAllLandmarksAsync(string? userId, string? search);

        Task<IEnumerable<GetAllLandmarksViewModel>> GetAllLandmarksForAdmin(string? userId, string? search);

        Task<IEnumerable<GetAllLandmarksViewModel>> GetAllLandmarksByDestinationIdAsync(string? userId, string? destId, string? search);

        Task<LandmarkDetailsViewModel> GetLandmarkDetailAsync(string? userId, string? landmarkId);

        Task<LandmarkEditViewModel> GetLandmarkForEditAsync(string? id);

        Task<bool> SaveEditChangesAsync(LandmarkEditViewModel? model);

        Task<bool> AddLandmarkAsync(AddLandmarkViewModel? model);

        Task DeleteOrRestoreLandmarkAsync(string? id);

        Task<IEnumerable<GetLandmarksForToursViewModel>> GetLandmarksForTourAsync(string? id);

    }
}
