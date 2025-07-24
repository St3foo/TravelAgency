using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.LandmarkModels;
using X.PagedList.Extensions;
using static TravelAgency.GCommon.Constants;

namespace TravelAgency.Controllers
{
    public class LandmarkController : BaseController
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(string? id, string? search, int page = 1)
        {

            try
            {
                IEnumerable<GetAllLandmarksViewModel> landmarks = await _landmarkService.GetAllLandmarksAsync(GetUserId());

                if (id != null)
                {
                    landmarks = await _landmarkService.GetAllLandmarksByDestinationIdAsync(GetUserId(), id);
                }

                if (!String.IsNullOrEmpty(search))
                {
                    landmarks = landmarks.Where(l => l.Name.Contains(search) || l.Destination.Contains(search));
                }

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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                LandmarkDetailsViewModel? landmark = await _landmarkService
                    .GetLandmarkDetailAsync(GetUserId(), id);

                return View(landmark);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Details");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                LandmarkEditViewModel? model = await _landmarkService.GetLandmarkForEditAsync(id);

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(LandmarkEditViewModel model)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return View(model);
                }

                bool result = await _landmarkService.SaveEditChangesAsync(model);

                if (result == false)
                {
                    return View(model);
                }

                return RedirectToAction(nameof(Details), new { id = model.Id });

            }
            catch (Exception e)
            {
                _logger.LogError(e, "EditPost");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add()
        {
            try
            {
                AddLandmarkViewModel landmark = new AddLandmarkViewModel
                {
                    Destinations = await _destinationService.GetAllDestinationsAsync(),
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(AddLandmarkViewModel? model)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return View(model);
                }

                bool result = await _landmarkService.AddLandmarkAsync(model);

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string? id)
        {
            try
            {
                DeleteLandmarkViewModel? model = await _landmarkService.GetLandmarkForDeleteAsync(id);

                if (model == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "DeleteGet");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(DeleteLandmarkViewModel? model)
        {
            try
            {
                bool result = await _landmarkService.DeleteLandmarkAsync(model);

                if (result == false)
                {
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "DeletePost");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
