using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.LandmarkModels;

namespace TravelAgency.Service.Core
{
    public class LandmarkService : ILandmarkService
    {
        private readonly TravelAgencyDbContext _dbContext;

        public LandmarkService(TravelAgencyDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<GetAllLandmarksViewModel>> GetAllLandmarksAsync(string? userId)
        {
            IEnumerable<GetAllLandmarksViewModel> landmarks = await _dbContext
                .Landmarks
                .AsNoTracking()
                .Select(l => new GetAllLandmarksViewModel 
                {
                    Id = l.Id,
                    Name = l.Name,
                    Destination = l.Destination.CountryName,
                    ImageUrl = l.ImageUrl,
                    FavoritesCount = l.UserLandmarks.Count,
                    IsFavorite = String.IsNullOrEmpty(userId) == false ?
                                 l.UserLandmarks.Any(ul => ul.UserId.ToLower() == userId.ToLower()) : false
                })
                .ToArrayAsync();

            return landmarks;
        }

        public async Task<LandmarkDetailsViewModel> GetLandmarkDetailAsync(string? landmarkId)
        {
            LandmarkDetailsViewModel? landmark = null;

            if (landmarkId != null)
            {
                Landmark? landmarkDetails = await _dbContext
                    .Landmarks
                    .Include(l => l.Destination)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(l => l.Id.ToString() == landmarkId);

                if (landmarkDetails != null)
                {
                    landmark = new LandmarkDetailsViewModel
                    {
                        Id = landmarkDetails.Id.ToString(),
                        Title = landmarkDetails.Name,
                        Description = landmarkDetails.Description,
                        Destination = landmarkDetails.Destination.CountryName,
                        ImageUrl = landmarkDetails.ImageUrl
                    };
                }
            }

            return landmark;
        }
    }
}
