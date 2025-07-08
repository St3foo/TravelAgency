using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.HotelModels;

namespace TravelAgency.Service.Core
{
    public class HotelService : IHotelInterface
    {
        private readonly TravelAgencyDbContext _context;

        public HotelService(TravelAgencyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<GetAllHotelsViewModel>> GetAllHotelsAsync()
        {
            IEnumerable<GetAllHotelsViewModel> hotels = await _context
                .Hotels
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
                })
                .ToArrayAsync();

            return hotels;
        }

        public async Task<HotelDetailsViewModel> GetHotelDetailsAsync(string id)
        {
            HotelDetailsViewModel? hotel = null;

            if (id != null) 
            {
                Hotel? details = await _context
                    .Hotels
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
    }
}
