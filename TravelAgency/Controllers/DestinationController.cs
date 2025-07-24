using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.DestinationModels;
using X.PagedList.Extensions;
using static TravelAgency.GCommon.Constants;

namespace TravelAgency.Controllers
{
    public class DestinationController : BaseController
    {
        private readonly IDestinationService _destinationService;
        private readonly ILogger<DestinationController> _logger;

        public DestinationController(IDestinationService destSevice, ILogger<DestinationController> logger)
        {
            _destinationService = destSevice;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            try
            {

                var destinations = await _destinationService.GetAllDestinationsAsync();

                if (!String.IsNullOrEmpty(search))
                {
                    destinations = destinations.Where(d => d.Name.Contains(search));
                }


                ViewBag.CurrentFilter = search;

                var pagedList = destinations.ToPagedList(page, PageSize);

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
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                DestinationDetailViewModel? destination = await _destinationService
                    .GetDestinationDetailsAsync(id);

                return View(destination);
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
                DestinationEditViewModel? model = await _destinationService
                    .GetDestinationForEditAsync(id);

                if (model == null)
                {
                    return RedirectToAction(nameof(Index));
                }

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
        public async Task<IActionResult> Edit(DestinationEditViewModel? model)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return View(model);
                }

                bool result = await _destinationService.SaveEditChangesAsync(model);

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
                AddDestinationViewModel model = new AddDestinationViewModel();

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "AddGet");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(AddDestinationViewModel? model)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return View(model);
                }

                bool result = await _destinationService.AddDestinationAsync(model);

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
                DeleteDestinationViewModel? model = await _destinationService.GetDestinationForDeleteAsync(id);

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
        public async Task<IActionResult> Delete(DeleteDestinationViewModel? model)
        {
            try
            {
                bool result = await _destinationService.DeleteDestinationAsync(model);

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
