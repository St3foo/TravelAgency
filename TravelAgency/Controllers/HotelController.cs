using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.HotelModels;

namespace TravelAgency.Controllers
{
    public class HotelController : BaseController
    {
        private readonly IHotelInterface _hotelInterface;

        public HotelController(IHotelInterface hotelInerface)
        {
            _hotelInterface = hotelInerface;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var hotels = await _hotelInterface.GetAllHotelsAsync();

                return View(hotels);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(string id) 
        {
            try
            {
                HotelDetailsViewModel? hotel = await _hotelInterface
                    .GetHotelDetailsAsync(id);

                return View(hotel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
