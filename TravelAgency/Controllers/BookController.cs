using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.Book;
using TravelAgency.ViewModels.Models.ReservationModels;
using X.PagedList.Extensions;
using static TravelAgency.GCommon.Constants;

namespace TravelAgency.Controllers
{
    public class BookController : BaseController
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
                var bookings = await _bookService.GetUserBookingsAsync(GetUserId());

                if (bookings == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var pagedList = bookings.ToPagedList(page, PageSize);

                return View(pagedList);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add(string? id)
        {
            try
            {
                var model = await _bookService.GetBookingDetailsForAddAsync(id);

                if (model == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Add");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddBookingViewModel model)
        {
            try
            {
                bool result = await _bookService.AddBookingAsync(GetUserId(), model);

                if (result == false)
                {
                    return RedirectToAction("Index", "Tour");
                }

                return RedirectToAction(nameof(Index));

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Create");
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
