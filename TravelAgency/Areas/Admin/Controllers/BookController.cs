using System.Drawing.Printing;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.Book;
using TravelAgency.ViewModels.Models.ReservationModels;
using X.PagedList.Extensions;
using static TravelAgency.GCommon.Constants;

namespace TravelAgency.Areas.Admin.Controllers
{
    public class BookController : BaseAdminController
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookService bookService, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                IEnumerable<GetAllBookingsViewModel> bookings = await _bookService.GetAllBookingsAsync();

                var pagedList = bookings.ToPagedList(page, PageSize);

                return View(pagedList);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string? id)
        {
            try
            {
                await _bookService.RemoveBookingAsync(id);

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
