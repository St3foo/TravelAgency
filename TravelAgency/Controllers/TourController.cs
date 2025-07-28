using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;
using X.PagedList.Extensions;
using static TravelAgency.GCommon.Constants;

namespace TravelAgency.Controllers
{
    public class TourController : BaseController
    {
        private readonly ITourService _tourService;
        private readonly ILogger<TourController> _logger;

        public TourController(ITourService tourService, ILogger<TourController> logger)
        {
            _tourService = tourService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            try
            {
                var tours = await _tourService.GetAllToursAsync();

                if (!String.IsNullOrEmpty(search))
                {
                    tours = tours.Where(t => t.Name.Contains(search) || t.Destination.Contains(search) || t.HotelName.Contains(search));
                }

                ViewBag.CurrentFilter = search;

                var pagedList = tours.ToPagedList(page, PageSize);

                return View(pagedList);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(string? id) 
        {
            try
            {
                var tour = await _tourService.GetTourDetailsAsync(id);

                return View(tour);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
