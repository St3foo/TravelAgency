using Microsoft.EntityFrameworkCore;
using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.TourModels;

namespace TravelAgency.Service.Core
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly ITourLandmarkRepository _tourLandmarkRepository;

        public TourService(ITourRepository tourRepository, ITourLandmarkRepository tourLandmarkRepository)
        {
            _tourRepository = tourRepository;
            _tourLandmarkRepository = tourLandmarkRepository;
        }

        public async Task<bool> AddTourAsync(AddTourViewModel? model)
        {
            bool result = false;

            if (model == null)
            {
                return result;
            }

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

            result = true;

            return result;
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

        public async Task<IEnumerable<GetAllToursViewModel>> GetAllToursAsync(string? search)
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

            if (!String.IsNullOrEmpty(search))
            {
                return tours.Where(t => t.Name.Contains(search) || t.Destination.Contains(search) || t.HotelName.Contains(search));
            }

            return tours;
        }

        public async Task<IEnumerable<GetAllToursViewModel>> GetAllToursForAdminAsync(string? search)
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

            if (!String.IsNullOrEmpty(search))
            {
                return tours.Where(t => t.Name.Contains(search) || t.HotelName.Contains(search) || t.Destination.Contains(search));
            }

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
                        ImageUrl = tour.ImageUrl,
                        DestinationName = tour.Destination.CountryName,
                        HotelName = tour.Hotel.HotelName,
                        Price = tour.Price,
                        Nights = tour.DaysStay,
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

        public async Task<TourEditViewModel> GetTourForEditAsync(string? id)
        {
            var tour = await _tourRepository
                .GetAllAttached()
                .Include(t => t.TourLandmarks)
                .IgnoreQueryFilters()
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id.ToString().ToLower() == id.ToLower());

            TourEditViewModel model = null;

            if (tour != null)
            {
                model = new TourEditViewModel
                {
                    Id = tour.Id,
                    Name = tour.Name,
                    Description = tour.Description,
                    ImageUrl = tour.ImageUrl,
                    Price = tour.Price,
                    DaysStay = tour.DaysStay,
                    DestinationId = tour.DestinationId,
                    HotelId = tour.HotelId,
                    Landmarks = tour.TourLandmarks.Select(l => l.LandmarkId)
                };
            }

            return model;
        }

        public async Task<bool> SaveEditChangesAsync(TourEditViewModel? model)
        {
            if (model == null)
            {
                return false;
            }

            var tour = await _tourRepository
                .GetAllAttached()
                .Include(t => t.TourLandmarks)
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(t => t.Id == model.Id);

            if (tour == null)
            {
                return false;
            }

            tour.Name = model.Name;
            tour.Description = model.Description;
            tour.ImageUrl = model.ImageUrl;
            tour.Price = model.Price;
            tour.DaysStay = model.DaysStay;
            tour.HotelId = model.HotelId;
            tour.DestinationId = model.DestinationId;

            foreach (var tl in tour.TourLandmarks)
            {
                await _tourLandmarkRepository.HardDeleteAsync(tl);
            }

            tour.TourLandmarks = model.Landmarks
                .Select(id => new TourLandmark
                {
                    TourId = tour.Id,
                    LandmarkId = id
                }).ToList();

            return await _tourRepository.UpdateAsync(tour);
        }
    }
}
