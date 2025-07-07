using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;

namespace TravelAgency.Controllers
{
    public class DestinationController : BaseController
    {
        private readonly IDestinationService _destinationService;

        public DestinationController(IDestinationService destSevice)
        {
            _destinationService = destSevice;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var destinations = await _destinationService.GetAllDestinationsAsync();

                return View(destinations);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
