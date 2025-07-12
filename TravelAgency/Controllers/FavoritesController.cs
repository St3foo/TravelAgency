using Microsoft.AspNetCore.Mvc;
using TravelAgency.Data.Models;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.FavoritesModels;

namespace TravelAgency.Controllers
{
    public class FavoritesController : BaseController
    {
        private readonly IFavoritesService _favoriteService;

        public FavoritesController(IFavoritesService favorites)
        {
            _favoriteService = favorites;
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
                Console.WriteLine(e.Message);
                throw;
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
                Console.WriteLine(e.Message);
                throw;
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
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
