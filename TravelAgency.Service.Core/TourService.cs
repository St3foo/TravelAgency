using Microsoft.EntityFrameworkCore;
using TravelAgency.Data.Models;
using TravelAgency.Data.Repository;
using TravelAgency.Data.Repository.Interfaces;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.TourModels;

namespace TravelAgency.Service.Core
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;

        public TourService(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
        }

        public async Task AddTourAsync(AddTourViewModel? model)
        {
            Tour tour = new Tour 
            {
                Name = model.Name,
                ImageUrl = model.ImageUrl,
                Description = model.Description,
                Price = model.Price,
                DaysStay = model.DaysStay,
                DestinationId = model.DestinationId,
                HotelId = model.HotelId,               
            };

            foreach (var l in model.Landmarks)
            {
                TourLandmark tourLandmark = new TourLandmark 
                {
                    LandmarkId = l
                };

                tour.TourLandmarks.Add(tourLandmark);
            }

            await _tourRepository.AddAsync(tour);
            await _tourRepository.SaveChangesAsync();

        }

        public async Task DeleteOrRestoreTourAsync(string? id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                Tour? tour = await _tourRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(t => t.Id.ToString().ToLower() == id.ToLower());

                if (tour != null)
                {
                    tour.IsDeleted = !tour.IsDeleted;

                    await _tourRepository
                        .UpdateAsync(tour);
                }
            }
        }

        public async Task<IEnumerable<GetAllToursViewModel>> GetAllToursAsync()
        {
            IEnumerable<GetAllToursViewModel> tours = await _tourRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(t => new GetAllToursViewModel 
                {
                    Id = t.Id,
                    Name = t.Name,
                    Destination = t.Destination.CountryName,
                    HotelName = t.Hotel.HotelName,
                    IsDeleted = t.IsDeleted,
                    ImageUrl = t.ImageUrl,
                    Nights = t.DaysStay,
                    Price = t.Price
                })
                .ToArrayAsync();

            return tours;
        }

        public async Task<IEnumerable<GetAllToursViewModel>> GetAllToursForAdminAsync()
        {
            IEnumerable<GetAllToursViewModel> tours = await _tourRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Select(t => new GetAllToursViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Destination = t.Destination.CountryName,
                    HotelName = t.Hotel.HotelName,
                    IsDeleted = t.IsDeleted,
                    ImageUrl = t.ImageUrl,
                    Nights = t.DaysStay,
                    Price = t.Price
                })
                .ToArrayAsync();

            return tours;
        }

        public async Task<TourDetailsViewModel> GetTourDetailsAsync(string? id)
        {
            TourDetailsViewModel? model = null;

            if (id != null)
            {
                Tour? tour = await _tourRepository
                    .GetAllAttached()
                    .Include(t => t.Destination)
                    .Include(t => t.Hotel)
                    .Include(t => t.TourLandmarks)
                    .ThenInclude(tl => tl.Landmark)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(t => t.Id.ToString().ToLower() == id.ToLower());

                if (tour != null)
                {
                    model = new TourDetailsViewModel
                    {
                        Id = tour.Id.ToString(),
                        Name = tour.Name,
                        Description = tour.Description,
                        ImageUrl= tour.ImageUrl,
                        DestinationName = tour.Destination.CountryName,
                        HotelName = tour.Hotel.HotelName,
                        Price = tour.Price,
                        Nights= tour.DaysStay,
                        Landmarks = tour.TourLandmarks.Select(t => new GetLandmarksForToursViewModel 
                        {
                            Id = t.LandmarkId,
                            Name = t.Landmark.Name,
                            ImageUrl = t.Landmark.ImageUrl,
                        })
                    };
                }
            }

            return model;
        }
    }
}
