using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.FavoritesModels;

namespace TravelAgency.Service.Core
{
    public class FavoritesService : IFavoritesService
    {
        private readonly IUserLandmarkRepository _userLandmarkRepository;
        private readonly UserManager<IdentityUser> _user;

        public FavoritesService(IUserLandmarkRepository userLandmarkRepository, UserManager<IdentityUser> user)
        {
            _userLandmarkRepository = userLandmarkRepository;
            _user = user;
        }

        public async Task AddToFavoritesAsync(string? userId, string? landmarkId)
        {
          
            if (landmarkId != null && userId != null) 
            {
                UserLandmark? favLandmark = await _userLandmarkRepository
                    .SingleOrDefaultAsync(ul => ul.UserId.ToLower() == userId.ToLower() && ul.LandmarkId.ToString() == landmarkId);

                if (favLandmark == null)
                {
                    UserLandmark landmarkToAdd = new UserLandmark 
                    {
                        UserId = userId,
                        LandmarkId = Guid.Parse(landmarkId)
                    };

                    await _userLandmarkRepository.AddAsync(landmarkToAdd);                   
                }
            }
        }

        public async Task<IEnumerable<GetAllFavoritesViewModel>> GetAllFavoritesLandmarksAsync(string? userId)
        {
            IdentityUser? user = await _user
                .FindByIdAsync(userId);

            IEnumerable<GetAllFavoritesViewModel> models = null;

            if (user != null)
            {
                models = await _userLandmarkRepository
                    .GetAllAttached()
                    .Include(ul => ul.Landmark)
                    .AsNoTracking()
                    .Where(ul => ul.UserId.ToLower() == userId.ToLower())
                    .Select(ul => new GetAllFavoritesViewModel 
                    {
                        Id = ul.LandmarkId,
                        Name = ul.Landmark.Name,
                        Location = ul.Landmark.LocationName,
                        ImageUrl = ul.Landmark.ImageUrl,
                        Destination = ul.Landmark.Destination.CountryName
                    })
                    .ToListAsync();
            }

            return models;
        }

        public async Task RemoveFromFavoritesAsync(string? userId, string? landmarkId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(landmarkId))
                return;

            UserLandmark? landmark = await _userLandmarkRepository
                .SingleOrDefaultAsync(ul => ul.UserId.ToLower() == userId.ToLower() && ul.LandmarkId.ToString() == landmarkId);

            if (landmark != null) 
            {
                await _userLandmarkRepository.HardDeleteAsync(landmark);
            }
        }
    }
}
