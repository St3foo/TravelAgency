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
        public async Task<IActionResult> Index(string? search, int page = 1)
        {

            try
            {
                IEnumerable<GetAllLandmarksViewModel> landmarks = await _landmarkService.GetAllLandmarksAsync(GetUserId());

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
        public async Task<IActionResult> GetByDestId(string? id, string? search, int page = 1) 
        {
            try
            {
                IEnumerable<GetAllLandmarksViewModel> landmarks = await _landmarkService.GetAllLandmarksByDestinationIdAsync(GetUserId(), id);

                if (landmarks == null)
                {
                    return RedirectToAction(nameof(Index));
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
    }
}
