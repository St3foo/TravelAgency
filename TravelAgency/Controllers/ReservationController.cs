﻿using System.Drawing.Printing;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.ReservationModels;
using X.PagedList.Extensions;
using static TravelAgency.GCommon.Constants;

namespace TravelAgency.Controllers
{
    public class ReservationController : BaseController
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(IReservationService service, ILogger<ReservationController> logger)
        {
            _reservationService = service;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                IEnumerable<GetUserReservationsViewModel> reservations = await _reservationService.GetUserReservationsAsync(GetUserId());

                if (reservations == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var pagedList = reservations.ToPagedList(page, PageSize);

                return View(pagedList);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Index");
                return RedirectToAction(nameof(Index));
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
                _logger.LogError(e, "Add");
                return RedirectToAction(nameof(Index));
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
                _logger.LogError(e, "Create");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string? id)
        {
            try
            {
                await _reservationService.RemoveFromReservationAsync(id);

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
