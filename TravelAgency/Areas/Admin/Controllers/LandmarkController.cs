using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.LandmarkModels;
using X.PagedList.Extensions;
using static TravelAgency.GCommon.Constants;

namespace TravelAgency.Areas.Admin.Controllers
{
    public class LandmarkController : BaseAdminController
    {
        private readonly ILandmarkService _landmarkService;
        private readonly IDestinationService _destinationService;
        private readonly ILogger<LandmarkController> _logger;

        public LandmarkController(ILandmarkService service, IDestinationService destinationService, ILogger<LandmarkController> logger)
        {
            _landmarkService = service;
            _destinationService = destinationService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1)
        {

            try
            {
                IEnumerable<GetAllLandmarksViewModel> landmarks = await _landmarkService.GetAllLandmarksForAdmin(GetUserId(), search);

                ViewBag.CurrentFilter = search;

                var pagedList = landmarks.ToPagedList(page, PageSize);

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
                LandmarkEditViewModel? model = await _landmarkService.GetLandmarkForEditAsync(id);

                if (model == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                model.Destinations = await _destinationService.GetAllDestinationsAsync(null);

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "EditGet");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LandmarkEditViewModel model)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    model.Destinations = await _destinationService.GetAllDestinationsAsync(null);
                    return View(model);
                }

                bool result = await _landmarkService.SaveEditChangesAsync(model);

                if (result == false)
                {
                    model.Destinations = await _destinationService.GetAllDestinationsAsync(null);
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
                AddLandmarkViewModel landmark = new AddLandmarkViewModel
                {
                    Destinations = await _destinationService.GetAllDestinationsAsync(null),
                };

                return View(landmark);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "AddGet");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddLandmarkViewModel? model)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    model.Destinations = await _destinationService.GetAllDestinationsAsync(null);
                    return View(model);
                }

                bool result = await _landmarkService.AddLandmarkAsync(model);

                if (result == false)
                {
                    model.Destinations = await _destinationService.GetAllDestinationsAsync(null);
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
                await _landmarkService.DeleteOrRestoreLandmarkAsync(id);

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
