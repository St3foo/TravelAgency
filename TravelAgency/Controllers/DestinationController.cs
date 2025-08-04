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

                var destinations = await _destinationService.GetAllDestinationsAsync(search);

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
 
    }
}
