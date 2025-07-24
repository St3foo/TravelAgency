using TravelAgency.Data.Models;

namespace TravelAgency.Data.Repository.Interfaces
{
    public interface IUserLandmarkRepository : IBaseRepository<UserLandmark, object>
    {
        bool Exists(string userId, string landmarkId);

        Task<bool> ExistsAsync(string userId, string landmarkId);
    }
}
