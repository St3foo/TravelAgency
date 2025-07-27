using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.LandmarkModels;

namespace TravelAgency.Service.Core
{
    public class LandmarkService : ILandmarkService
    {
        private readonly ILandmarkRepository _landmarkRepository;
        private readonly IDestinationRepository _destinationRepository;

        public LandmarkService(ILandmarkRepository landmarkRepository, IDestinationRepository destinationRepository)
        {
            _landmarkRepository = landmarkRepository;
            _destinationRepository = destinationRepository;
        }

        public async Task<bool> AddLandmarkAsync(AddLandmarkViewModel? model)
        {
            bool result = false;

            Destination? destination = await _destinationRepository
                .SingleOrDefaultAsync(d => d.Id.ToString() == model.DestinationId);

            if (destination != null)
            {
                Landmark landmark = new Landmark 
                {
                    Name = model.Name,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl,
                    LocationName = model.Location,
                    DestinationId = Guid.Parse(model.DestinationId)
                };

                await _landmarkRepository.AddAsync(landmark);

                result = true;
            }

            return result;
        }

        public async Task DeleteOrRestoreLandmarkAsync(string? id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                Landmark? landmark = await _landmarkRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(l => l.Id.ToString().ToLower() == id.ToLower());

                if (landmark != null)
                {
                    landmark.IsDeleted = !landmark.IsDeleted;

                    await _landmarkRepository
                        .UpdateAsync(landmark);
                }
            }
        }

        public async Task<IEnumerable<GetAllLandmarksViewModel>> GetAllLandmarksAsync(string? userId)
        {
            IEnumerable<GetAllLandmarksViewModel> landmarks = await _landmarkRepository
                .GetAllAttached()
                .Include(l => l.Destination)
                .AsNoTracking()
                .Select(l => new GetAllLandmarksViewModel 
                {
                    Id = l.Id,
                    Name = l.Name,
                    Destination = l.Destination.CountryName,
                    ImageUrl = l.ImageUrl,
                    FavoritesCount = l.UserLandmarks.Count,
                    IsFavorite = String.IsNullOrEmpty(userId) == false ?
                                 l.UserLandmarks.Any(ul => ul.UserId.ToLower() == userId.ToLower()) : false,
                    IsDeleted = l.IsDeleted
                })
                .ToArrayAsync();

            return landmarks;
        }

        public async Task<IEnumerable<GetAllLandmarksViewModel>> GetAllLandmarksByDestinationIdAsync(string? userId, string? destId)
        {
            IEnumerable<GetAllLandmarksViewModel> landmarks = await _landmarkRepository
                .GetAllAttached()
                .AsNoTracking()
                .Where(l => l.DestinationId.ToString() == destId)
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

        public async Task<IEnumerable<GetAllLandmarksViewModel>> GetAllLandmarksForAdmin(string? userId)
        {
            IEnumerable<GetAllLandmarksViewModel> landmarks = await _landmarkRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .Include(l => l.Destination)
                .AsNoTracking()
                .Select(l => new GetAllLandmarksViewModel
                {
                    Id = l.Id,
                    Name = l.Name,
                    Destination = l.Destination.CountryName,
                    ImageUrl = l.ImageUrl,
                    FavoritesCount = l.UserLandmarks.Count,
                    IsFavorite = String.IsNullOrEmpty(userId) == false ?
                                 l.UserLandmarks.Any(ul => ul.UserId.ToLower() == userId.ToLower()) : false,
                    IsDeleted = l.IsDeleted
                })
                .ToArrayAsync();

            return landmarks;
        }

        public async Task<LandmarkDetailsViewModel> GetLandmarkDetailAsync(string? userId, string? landmarkId)
        {
            LandmarkDetailsViewModel? landmark = null;

            if (landmarkId != null)
            {
                Landmark? landmarkDetails = await _landmarkRepository
                    .GetAllAttached()
                    .Include(l => l.Destination)
                    .Include(l => l.UserLandmarks)
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
                        ImageUrl = landmarkDetails.ImageUrl,
                        IsFavorite = String.IsNullOrEmpty(userId) == false ?
                                 landmarkDetails.UserLandmarks.Any(ul => ul.UserId.ToLower() == userId.ToLower()) : false
                    };
                }
            }

            return landmark;
        }

        public async Task<LandmarkEditViewModel> GetLandmarkForEditAsync(string? id)
        {
            LandmarkEditViewModel? landmark = null;

            Landmark? landmarkToEdit = await _landmarkRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
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
                Landmark? landmark = await _landmarkRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(l => l.Id.ToString() == model.Id);

                Destination? destination = await _destinationRepository
                    .SingleOrDefaultAsync(d => d.Id.ToString() == model.DestinationId);

                if (landmark != null && destination != null)
                {
                    landmark.Name = model.Name;
                    landmark.Description = model.Description;
                    landmark.ImageUrl = model.ImageUrl;
                    landmark.LocationName = model.Location;
                    landmark.DestinationId = Guid.Parse(model.DestinationId);

                    result = await _landmarkRepository.UpdateAsync(landmark);
                }
            }

            return result;
        }
    }
}
