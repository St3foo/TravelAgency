using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.HotelModels;

namespace TravelAgency.Service.Core
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IDestinationRepository _destinationRepository;

        public HotelService(IHotelRepository hotelRepository, IDestinationRepository destinationRepository)
        {
            _hotelRepository = hotelRepository;
            _destinationRepository = destinationRepository;
        }

        public async Task<bool> AddHotelAsync(AddHotelViewModel? model)
        {
            bool result = false;

            Destination? destination = await _destinationRepository
                .SingleOrDefaultAsync(d => d.Id.ToString() == model.DestinationId);

            if (destination != null)
            {
                Hotel hotel = new Hotel 
                {
                    HotelName = model.Name,
                    CityName = model.CityName,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl,
                    DestinationId = destination.Id,
                    Price = model.Price,
                    DaysStay = model.Nights
                };

                await _hotelRepository.AddAsync(hotel);

                result = true;
            }

            return result;
        }

        public async Task DeleteOrRestoreHotelAsync(string? id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                Hotel? hotel = await _hotelRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(h => h.Id.ToString().ToLower() == id.ToLower());
                if (hotel != null)
                {
                    hotel.IsDeleted = !hotel.IsDeleted;

                    await _hotelRepository
                        .UpdateAsync(hotel);
                }
            }
        }

        public async Task<IEnumerable<GetAllHotelsViewModel>> GetAllHotelsAsync()
        {
            IEnumerable<GetAllHotelsViewModel> hotels = await _hotelRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(h => new GetAllHotelsViewModel
                {
                    Id = h.Id,
                    Name = h.HotelName,
                    Destination = h.Destination.CountryName,
                    City = h.CityName,
                    ImageUrl = h.ImageUrl,
                    Nights = h.DaysStay,
                    Price = h.Price,
                    IsDeleted = h.IsDeleted,
                })
                .ToArrayAsync();

            return hotels;
        }

        public async Task<IEnumerable<GetAllHotelsViewModel>> GetAllHotelsByDestinationIdAsync(string? id)
        {
            IEnumerable<GetAllHotelsViewModel> hotels = await _hotelRepository
                .GetAllAttached()
                .AsNoTracking()
                .Where(h => h.DestinationId.ToString() == id)
                .Select(h => new GetAllHotelsViewModel
                {
                    Id = h.Id,
                    Name = h.HotelName,
                    Destination = h.Destination.CountryName,
                    City = h.CityName,
                    ImageUrl = h.ImageUrl,
                    Nights = h.DaysStay,
                    Price = h.Price,
                })
                .ToArrayAsync();

            return hotels;
        }

        public async Task<IEnumerable<GetAllHotelsViewModel>> GetAllHotelsForAdminAsync()
        {
            IEnumerable<GetAllHotelsViewModel> hotels = await _hotelRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Select(h => new GetAllHotelsViewModel
                {
                    Id = h.Id,
                    Name = h.HotelName,
                    Destination = h.Destination.CountryName,
                    City = h.CityName,
                    ImageUrl = h.ImageUrl,
                    Nights = h.DaysStay,
                    Price = h.Price,
                    IsDeleted = h.IsDeleted,
                })
                .ToArrayAsync();

            return hotels;
        }

        public async Task<HotelDetailsViewModel> GetHotelDetailsAsync(string id)
        {
            HotelDetailsViewModel? hotel = null;

            if (id != null) 
            {
                Hotel? details = await _hotelRepository
                    .GetAllAttached()
                    .Include(h => h.Destination)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(h => h.Id.ToString() == id);

                if (details != null)
                {
                    hotel = new HotelDetailsViewModel 
                    {
                        Id = details.Id.ToString(),
                        Title = details.HotelName,
                        Destination = details.Destination.CountryName,
                        Description = details.Description,
                        ImageUrl = details.ImageUrl,
                        Price = details.Price,
                        Nights = details.DaysStay,
                        City = details.CityName
                    };
                }
            }

            return hotel;
        }

        public async Task<HotelEditViewModel> GetHotelForEditAsync(string? id)
        {
            HotelEditViewModel? hotel = null;

            var hotelInfo = await _hotelRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .SingleOrDefaultAsync(h => h.Id.ToString() == id);

            if (hotelInfo != null)
            {
                hotel = new HotelEditViewModel
                {
                    Id = hotelInfo.Id.ToString(),
                    Name = hotelInfo.HotelName,
                    CityName = hotelInfo.CityName,
                    ImageUrl = hotelInfo.ImageUrl,
                    Description = hotelInfo.Description,
                    Price = hotelInfo.Price,
                    Nights= hotelInfo.DaysStay,
                    DestinationId = hotelInfo.DestinationId.ToString()
                };
            }

            return hotel;
        }

        public async Task<bool> SaveEditChangesAsync(HotelEditViewModel? model)
        {
            bool result = false;

            if (model != null)
            {
                Hotel? hotel = await _hotelRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(h => h.Id.ToString() == model.Id);

                Destination? destination = await _destinationRepository
                    .SingleOrDefaultAsync(d => d.Id.ToString() == model.DestinationId);

                if (hotel != null && destination != null)
                {
                    hotel.HotelName = model.Name;
                    hotel.ImageUrl = model.ImageUrl;
                    hotel.Description = model.Description;
                    hotel.CityName = model.CityName;
                    hotel.Price = model.Price;
                    hotel.DaysStay = model.Nights;
                    hotel.DestinationId = Guid.Parse(model.DestinationId);

                    result = await _hotelRepository.UpdateAsync(hotel);
                }
            }

            return result;
        }
    }
}
