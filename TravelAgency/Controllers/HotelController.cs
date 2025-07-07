using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;

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
    }
}
