using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
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
    }
}
