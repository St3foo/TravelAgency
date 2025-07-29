using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.HotelModels;
using X.PagedList.Extensions;
using static TravelAgency.GCommon.Constants;

namespace TravelAgency.Areas.Admin.Controllers
{
    public class HotelController : BaseAdminController
    {
        private readonly IHotelService _hotelService;
        private readonly IDestinationService _destinationService;
        private readonly ILogger<HotelController> _logger;

        public HotelController(IHotelService hotelService, IDestinationService destinationService, ILogger<HotelController> logger)
        {
            _hotelService = hotelService;
            _destinationService = destinationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? id, string? search, int page = 1)
        {
            try
            {

                IEnumerable<GetAllHotelsViewModel> hotels = await _hotelService.GetAllHotelsForAdminAsync();

                if (id != null)
                {
                    hotels = await _hotelService.GetAllHotelsByDestinationIdAsync(id);
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

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                HotelEditViewModel? model = await _hotelService.GetHotelForEditAsync(id);

                if (model == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                model.Destinations = await _destinationService.GetAllDestinationsAsync();

                return View(model);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "EditGet");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(HotelEditViewModel? model)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return View(model);
                }

                bool result = await _hotelService.SaveEditChangesAsync(model);

                if (result == false)
                {
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "EditPost");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                AddHotelViewModel model = new AddHotelViewModel
                {
                    Destinations = await _destinationService.GetAllDestinationsAsync()
                };

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "AddGet");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddHotelViewModel? model)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return View(model);
                }

                bool result = await _hotelService.AddHotelAsync(model);

                if (result == false)
                {
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "AddPost");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> ToggleDelete(string? id)
        {
            try
            {
                await _hotelService.DeleteOrRestoreHotelAsync(id);

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
