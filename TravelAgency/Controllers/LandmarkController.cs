using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.LandmarkModels;
using X.PagedList.Extensions;

namespace TravelAgency.Controllers
{
    public class LandmarkController : BaseController
    {
        private readonly ILandmarkService _landmarkService;
        private readonly IDestinationService _destinationService;

        public LandmarkController(ILandmarkService service, IDestinationService destinationService)
        {
            _landmarkService = service;
            _destinationService = destinationService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(string? id, string? search, int page = 1)
        {

            const int pageSize = 8;

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

                var pagedList = landmarks.ToPagedList(page, pageSize);

                return View(pagedList);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
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
                Console.WriteLine(e.Message);
                throw;
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
                Console.WriteLine(e.Message);
                throw;
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
                Console.WriteLine(e.Message);
                throw;
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
                Console.WriteLine(e.Message);
                throw;
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
                Console.WriteLine(e.Message);
                throw;
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
                Console.WriteLine(e.Message);
                throw;
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
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
