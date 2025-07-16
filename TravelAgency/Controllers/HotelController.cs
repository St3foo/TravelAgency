using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.HotelModels;

namespace TravelAgency.Controllers
{
    public class HotelController : BaseController
    {
        private readonly IHotelInterface _hotelInterface;
        private readonly IDestinationService _destinationService;

        public HotelController(IHotelInterface hotelInerface, IDestinationService destinationService)
        {
            _hotelInterface = hotelInerface;
            _destinationService = destinationService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(string? id)
        {
            try
            {

                IEnumerable<GetAllHotelsViewModel> hotels = null; 

                if (id == null)
                {
                    hotels = await _hotelInterface.GetAllHotelsAsync();
                }
                else
                {
                    hotels = await _hotelInterface.GetAllHotelsByDestinationIdAsync(id);
                }

                    return View(hotels);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(string id) 
        {
            try
            {
                HotelDetailsViewModel? hotel = await _hotelInterface
                    .GetHotelDetailsAsync(id);

                return View(hotel);
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
                HotelEditViewModel? model = await _hotelInterface.GetHotelForEditAsync(id);

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
        public async Task<IActionResult> Edit(HotelEditViewModel? model)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return View(model);
                }

                bool result = await _hotelInterface.SaveEditChangesAsync(model);

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
                AddHotelViewModel model = new AddHotelViewModel
                {
                    Destinations = await _destinationService.GetAllDestinationsAsync()
                };

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
        public async Task<IActionResult> Add(AddHotelViewModel? model) 
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return View(model);
                }

                bool result = await _hotelInterface.AddHotelAsync(model);

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
                DeleteHotelViewModel? model = await _hotelInterface.GetHotelForDeleteAsync(id);

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
        public async Task<IActionResult> Delete(DeleteHotelViewModel? model) 
        {
            try
            {
                bool result = await _hotelInterface.DeleteHotelAsync(model);

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
