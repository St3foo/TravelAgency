using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.LandmarkModels;

namespace TravelAgency.Controllers
{
    public class LandmarkController : BaseController
    {
        private readonly ILandmarkService _landmarkService;

        public LandmarkController(ILandmarkService service)
        {
            _landmarkService = service;
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
    }
}
