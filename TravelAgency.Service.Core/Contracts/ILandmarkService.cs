using TravelAgency.ViewModels.Models.LandmarkModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface ILandmarkService
    {
        Task<IEnumerable<GetAllLandmarksViewModel>> GetAllLandmarksAsync(string? userId);

        Task<IEnumerable<GetAllLandmarksViewModel>> GetAllLandmarksByDestinationIdAsync(string? userId, string? destId);

        Task<LandmarkDetailsViewModel> GetLandmarkDetailAsync(string? userId, string? landmarkId);

        Task<LandmarkEditViewModel> GetLandmarkForEditAsync(string? id);

        Task<bool> SaveEditChangesAsync(LandmarkEditViewModel? model);

        Task<bool> AddLandmarkAsync(AddLandmarkViewModel? model);

        Task<DeleteLandmarkViewModel> GetLandmarkForDeleteAsync(string? id);

        Task<bool> DeleteLandmarkAsync(DeleteLandmarkViewModel? model);
    }
}
