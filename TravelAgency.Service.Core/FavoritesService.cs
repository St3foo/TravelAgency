using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.FavoritesModels;

namespace TravelAgency.Service.Core
{
    public class FavoritesService : IFavoritesService
    {
        private readonly TravelAgencyDbContext _context;
        private readonly UserManager<IdentityUser> _user;

        public FavoritesService(TravelAgencyDbContext context, UserManager<IdentityUser> user)
        {
            _context = context;
            _user = user;
        }

        public async Task AddToFavoritesAsync(string? userId, string? landmarkId)
        {

            IdentityUser? user = await _user
                .FindByIdAsync(userId);

            Landmark? landmark = await _context
                .Landmarks
                .SingleOrDefaultAsync(l => l.Id.ToString() == landmarkId);

            if (landmark != null && user != null) 
            {
                UserLandmark? favLandmark = await _context
                    .UsersLandmarks
                    .SingleOrDefaultAsync(ul => ul.UserId.ToLower() == userId.ToLower() && ul.LandmarkId == landmark.Id);

                if (favLandmark == null)
                {
                    UserLandmark landmarkToAdd = new UserLandmark 
                    {
                        UserId = userId,
                        LandmarkId = landmark.Id,
                    };

                    await _context.UsersLandmarks.AddAsync(landmarkToAdd);
                    await _context.SaveChangesAsync();

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
                models = await _context
                    .UsersLandmarks
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
            UserLandmark? landmark = await _context
                .UsersLandmarks
                .SingleOrDefaultAsync(ul => ul.UserId.ToLower() == userId.ToLower() && ul.LandmarkId.ToString() == landmarkId);

            if (landmark != null) 
            {
                _context.UsersLandmarks.Remove(landmark);
                await _context.SaveChangesAsync();
            }
        }
    }
}
