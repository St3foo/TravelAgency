using TravelAgency.ViewModels.Models.LandmarkModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface ILandmarkService
    {
        Task<IEnumerable<GetAllLandmarksViewModel>> GetAllLandmarksAsync(string? userId);

        Task<LandmarkDetailsViewModel> GetLandmarkDetailAsync(string? landmarkId);

        Task<LandmarkEditViewModel> GetLandmarkForEditAsync(string? id);

        Task<bool> SaveEditChangesAsync(LandmarkEditViewModel? model);

        Task<bool> AddLandmarkAsync(AddLandmarkViewModel? model);
    }
}
