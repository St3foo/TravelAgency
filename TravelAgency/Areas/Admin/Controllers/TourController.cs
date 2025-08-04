using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.DestinationModels;
using TravelAgency.ViewModels.Models.TourModels;
using X.PagedList.Extensions;
using static TravelAgency.GCommon.Constants;

namespace TravelAgency.Areas.Admin.Controllers
{
    public class TourController : BaseAdminController
    {
        private readonly ITourService _tourService;
        private readonly IDestinationService _destinationService;
        private readonly IHotelService _hotelService;
        private readonly ILandmarkService _landmarkService;
        private readonly ILogger<TourController> _logger;

        public TourController(ITourService tourService, ILogger<TourController> logger,
            IDestinationService destinationService, IHotelService hotelService,
            ILandmarkService landmarkService)
        {
            _tourService = tourService;
            _logger = logger;
            _destinationService = destinationService;
            _hotelService = hotelService;
            _landmarkService = landmarkService;
        }

        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            try
            {

                IEnumerable<GetAllToursViewModel> tours = await _tourService.GetAllToursForAdminAsync(search);

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

        [HttpGet]
        public async Task<IActionResult> Add(string? search)
        {
            try
            {
                IEnumerable<AllDestinationsViewModel> destinations = await _destinationService.GetAllDestinationsForAdminAsync(null);

                if (!String.IsNullOrEmpty(search))
                {
                    destinations = destinations.Where(d => d.Name.Contains(search));
                }

                ViewBag.CurrentFilter = search;

                return View(destinations);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create(string? id)
        {
            try
            {
                AddTourViewModel? model = null;

                if (id != null)
                {
                    model = new AddTourViewModel
                    {
                        DestinationId = Guid.Parse(id)
                    };
                }

                model.AllHotels = await _hotelService.GetHotelsForTourAsync(id);
                model.AllLanadmarks = await _landmarkService.GetLandmarksForTourAsync(id);

                return View(model);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddTourViewModel? model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.AllHotels = await _hotelService.GetHotelsForTourAsync(model.DestinationId.ToString());
                    model.AllLanadmarks = await _landmarkService.GetLandmarksForTourAsync(model.DestinationId.ToString());
                    return View(model);
                }

                await _tourService.AddTourAsync(model);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            try
            {
                TourEditViewModel? model = await _tourService.GetTourForEditAsync(id);

                if (model == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                model.AllHotels = await _hotelService.GetHotelsForTourAsync(model.DestinationId.ToString());
                model.AllLanadmarks = await _landmarkService.GetLandmarksForTourAsync(model.DestinationId.ToString());

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TourEditViewModel? model)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    model.AllHotels = await _hotelService.GetHotelsForTourAsync(model.DestinationId.ToString());
                    model.AllLanadmarks = await _landmarkService.GetLandmarksForTourAsync(model.DestinationId.ToString());
                    return View(model);
                }

                bool result = await _tourService.SaveEditChangesAsync(model);

                if (result == false)
                {
                    model.AllHotels = await _hotelService.GetHotelsForTourAsync(model.DestinationId.ToString());
                    model.AllLanadmarks = await _landmarkService.GetLandmarksForTourAsync(model.DestinationId.ToString());
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
