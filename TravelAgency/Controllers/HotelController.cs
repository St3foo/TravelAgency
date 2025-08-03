using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.HotelModels;
using X.PagedList.Extensions;
using static TravelAgency.GCommon.Constants;

namespace TravelAgency.Controllers
{
    public class HotelController : BaseController
    {
        private readonly IHotelService _hotelInterface;
        private readonly IDestinationService _destinationService;
        private readonly ILogger<HotelController> _logger;

        public HotelController(IHotelService hotelInerface, IDestinationService destinationService, ILogger<HotelController> logger)
        {
            _hotelInterface = hotelInerface;
            _destinationService = destinationService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            try
            {

                IEnumerable<GetAllHotelsViewModel> hotels = await _hotelInterface.GetAllHotelsAsync();

                if (!String.IsNullOrEmpty(search))
                {
                    hotels = hotels.Where(h => h.Name.Contains(search) || h.City.Contains(search) || h.Destination.Contains(search));
                }

                ViewBag.CurrentFilter = search;

                var pagedList = hotels.ToPagedList(page, PageSize);

                return View(pagedList);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Index");
                return RedirectToAction(nameof(Index));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Filter(string? id,string? search, int page = 1) 
        {
            try
            {
                IEnumerable<GetAllHotelsViewModel> hotels = await _hotelInterface.GetAllHotelsByDestinationIdAsync(id);

                if (hotels == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (!String.IsNullOrEmpty(search))
                {
                    hotels = hotels.Where(h => h.Name.Contains(search) || h.City.Contains(search) || h.Destination.Contains(search));
                }

                ViewBag.CurrentFilter = search;

                var pagedList = hotels.ToPagedList(page, PageSize);

                return View(pagedList);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Index");
                return RedirectToAction(nameof(Index));
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
                _logger.LogError(e, "Details");
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
