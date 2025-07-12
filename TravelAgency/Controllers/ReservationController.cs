using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.ReservationModels;

namespace TravelAgency.Controllers
{
    public class ReservationController : BaseController
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService service)
        {
            _reservationService = service;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<GetAllReservationsViewModel> reservations = await _reservationService.GetAllReservationsAsync(GetUserId());

                if (reservations == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(reservations);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add(string? id)
        {
            try
            {
                AddReservationViewModel? model = await _reservationService.GetReservationDetailsAsync(id);

                if (model == null)
                {
                    return RedirectToAction("Index", "Hotel");
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
        public async Task<IActionResult> Create(AddReservationViewModel model)
        {
            try
            {
                bool result = await _reservationService.AddReservationAsync(GetUserId(), model);

                if (result == false)
                {
                    return RedirectToAction("Index", "Hotel");
                }

                return RedirectToAction(nameof(Index));
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
                await _reservationService.RemoveFromFavoritesAsync(id);

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
