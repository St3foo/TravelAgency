using Microsoft.EntityFrameworkCore;
using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;

namespace TravelAgency.Data.Repository
{
    public class UserLandmarkRepository : BaseRepository<UserLandmark, object>, IUserLandmarkRepository
    {
        public UserLandmarkRepository(TravelAgencyDbContext context) 
            : base(context)
        {
        }

        public bool Exists(string userId, string landmarkId)
        {
            return this.GetAllAttached()
                .Any(ul => ul.UserId.ToLower() == userId.ToLower() &&
                            ul.LandmarkId.ToString().ToLower() == landmarkId.ToLower());
        }

        public Task<bool> ExistsAsync(string userId, string landmarkId)
        {
            return this.GetAllAttached()
                .AnyAsync(ul => ul.UserId.ToLower() == userId.ToLower() &&
                            ul.LandmarkId.ToString().ToLower() == landmarkId.ToLower());
        }
    }
}
