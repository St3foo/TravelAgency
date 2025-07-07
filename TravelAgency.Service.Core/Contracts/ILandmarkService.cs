using TravelAgency.ViewModels.Models.LandmarkModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface ILandmarkService
    {
        Task<IEnumerable<GetAllLandmarksViewModel>> GetAllLandmarksAsync(string? userId);
    }
}
