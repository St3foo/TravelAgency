using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.TourModels;
using X.PagedList.Extensions;
using static TravelAgency.GCommon.Constants;

namespace TravelAgency.Areas.Admin.Controllers
{
    public class TourController : BaseAdminController
    {
        private readonly ITourService _tourService;
        private readonly ILogger<TourController> _logger;

        public TourController(ITourService tourService, ILogger<TourController> logger)
        {
            _tourService = tourService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            try
            {

                IEnumerable<GetAllToursViewModel> tours = await _tourService.GetAllToursForAdminAsync();


                if (!String.IsNullOrEmpty(search))
                {
                    tours = tours.Where(t => t.Name.Contains(search) || t.HotelName.Contains(search) || t.Destination.Contains(search));
                }

                ViewBag.CurrentFilter = search;

                var pagedList = tours.ToPagedList(page, PageSize);

                return View(pagedList);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Index");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> ToggleDelete(string? id)
        {
            try
            {
                await _tourService.DeleteOrRestoreTourAsync(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ToggleDelete");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
