using Microsoft.AspNetCore.Mvc;
using TravelAgency.Data.Models;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.FavoritesModels;

namespace TravelAgency.Controllers
{
    public class FavoritesController : BaseController
    {
        private readonly IFavoritesService _favoriteService;
        private readonly ILogger<FavoritesController> _logger;

        public FavoritesController(IFavoritesService favorites, ILogger<FavoritesController> logger)
        {
            _favoriteService = favorites;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<GetAllFavoritesViewModel> model = await _favoriteService.GetAllFavoritesLandmarksAsync(GetUserId());

                if (model == null) 
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Index");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(string? id) 
        {
            try
            {
                await _favoriteService.AddToFavoritesAsync(GetUserId(), id);

                return RedirectToAction("Index" , "Landmark");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Add");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string? id)
        {
            try
            {
                await _favoriteService.RemoveFromFavoritesAsync(GetUserId(), id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Remove");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
