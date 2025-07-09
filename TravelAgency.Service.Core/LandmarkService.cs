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

        public async Task<LandmarkEditViewModel> GetLandmarkForEditAsync(string? id)
        {
            LandmarkEditViewModel? landmark = null;

            Landmark? landmarkToEdit = await _dbContext
                .Landmarks
                .AsNoTracking()
                .SingleOrDefaultAsync(l => l.Id.ToString() == id);

            if (landmarkToEdit != null)
            {
                landmark = new LandmarkEditViewModel
                {
                    Id = landmarkToEdit.Id.ToString(),
                    Name = landmarkToEdit.Name,
                    Description = landmarkToEdit.Description,
                    ImageUrl = landmarkToEdit.ImageUrl,
                    Location = landmarkToEdit.LocationName,
                    DestinationId = landmarkToEdit.DestinationId.ToString()
                };
            }

            return landmark;
        }

        public async Task<bool> SaveEditChangesAsync(LandmarkEditViewModel? model)
        {
            bool result = false;

            if (model != null)
            {
                Landmark? landmark = await _dbContext
                    .Landmarks
                    .SingleOrDefaultAsync(l => l.Id.ToString() == model.Id);

                Destination? destination = await _dbContext
                    .Destinations
                    .SingleOrDefaultAsync(d => d.Id.ToString() == model.Id);

                if (landmark != null && destination != null)
                {
                    landmark.Name = model.Name;
                    landmark.Description = model.Description;
                    landmark.ImageUrl = model.ImageUrl;
                    landmark.LocationName = model.Location;
                    landmark.DestinationId = Guid.Parse(model.DestinationId);

                    await _dbContext.SaveChangesAsync();
                    result = true;
                }
            }

            return result;
        }
    }
}
