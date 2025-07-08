using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.LandmarkModels;

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
        public async Task<IActionResult> Index()
        {
            try
            {
                var landmarks = await _landmarkService.GetAllLandmarksAsync(GetUserId());

                return View(landmarks);
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
                    .GetLandmarkDetailAsync(id);

                return View(landmark);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
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
        public async Task<IActionResult> Edit(LandmarkEditViewModel model)
        {
            try
            {
                if(!this.ModelState.IsValid)
                {
                    return View(model);
                }

                await _landmarkService.SaveEditChangesAsync(model);

                return RedirectToAction(nameof(Details), new { id = model.Id });

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
