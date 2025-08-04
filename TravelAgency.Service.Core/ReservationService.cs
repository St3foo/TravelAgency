using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.ReservationModels;

namespace TravelAgency.Service.Core
{
    public class ReservationService : IReservationService
    {
        private readonly IUserHotelRepository _userHotelRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly UserManager<IdentityUser> _user;

        public ReservationService(IUserHotelRepository userHotelRepository, IHotelRepository hotelRepository, UserManager<IdentityUser> user)
        {
            _userHotelRepository = userHotelRepository;
            _hotelRepository = hotelRepository;
            _user = user;
        }

        public async Task<bool> AddReservationAsync(string? userId, AddReservationViewModel? model)
        {
            bool result = false;

            IdentityUser? user = await _user.FindByIdAsync(userId);

            Hotel? hotel = await _hotelRepository
                .GetAllAttached()
                .AsNoTracking()
                .SingleOrDefaultAsync(h => h.Id == model.Id);

            if (user != null && hotel != null)
            {
                UserHotel reservation = new UserHotel
                {
                    UserId = userId,
                    HotelId = model.Id,
                    StartDate = model.ReservationDate,
                    EndDate = model.ReservationDate.AddDays(model.Nights)
                };

                await _userHotelRepository.AddAsync(reservation);

                result = true;
            }

            return result;
        }

        public async Task<IEnumerable<GetUserReservationsViewModel>> GetUserReservationsAsync(string? userId)
        {
            IdentityUser? user = await _user.FindByIdAsync(userId);

            IEnumerable<GetUserReservationsViewModel> reservations = null!;

            if (user != null)
            {
                reservations = await _userHotelRepository
                    .GetAllAttached()
                    .Include(uh => uh.Hotel)
                    .AsNoTracking()
                    .Where(uh => uh.UserId.ToLower() == userId.ToLower())
                    .Select(uh => new GetUserReservationsViewModel
                    {
                        Id = uh.Id,
                        HotelId = uh.HotelId,
                        HotelName = uh.Hotel.HotelName,
                        Location = uh.Hotel.CityName,
                        Destination = uh.Hotel.Destination.CountryName,
                        ImageUrl = uh.Hotel.ImageUrl,
                        Price = uh.Hotel.Price,
                        StartDate = uh.StartDate,
                        EndDate = uh.EndDate
                    })
                    .OrderBy(uh => uh.StartDate)
                    .ToListAsync();
            }

            return reservations;
        }

        public async Task<AddReservationViewModel> GetReservationDetailsForAddAsync(string? hotelId)
        {
            AddReservationViewModel? model = null;

            if (hotelId != null)
            {
                Hotel? hotel = await _hotelRepository
                    .GetAllAttached()
                    .Include(h => h.Destination)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(h => h.Id.ToString() == hotelId);

                if (hotel != null)
                {
                    model = new AddReservationViewModel
                    {
                        Id = hotel.Id,
                        Name = hotel.HotelName,
                        City = hotel.CityName,
                        Destination = hotel.Destination.CountryName,
                        ImageUrl = hotel.ImageUrl,
                        Price = hotel.Price,
                        Nights = hotel.DaysStay,
                        ReservationDate = DateTime.Now
                    };
                }
            }

            return model;
        }

        public async Task RemoveFromReservationAsync(string? reservationId)
        {
            UserHotel? reservation = await _userHotelRepository
                .SingleOrDefaultAsync(uh => uh.Id.ToString() == reservationId);

            if (reservation != null)
            {
                await _userHotelRepository.HardDeleteAsync(reservation);
            }
        }

        public async Task<IEnumerable<GetAllReservationViewModel>> GetAllReservationsAsync()
        {
            IEnumerable<GetAllReservationViewModel> reservations = await _userHotelRepository
                    .GetAllAttached()
                    .Include(uh => uh.Hotel)
                    .AsNoTracking()
                    .Select(uh => new GetAllReservationViewModel
                    {
                        Id = uh.Id,
                        UserName = uh.User.NormalizedUserName,
                        HotelId = uh.HotelId,
                        HotelName = uh.Hotel.HotelName,
                        Location = uh.Hotel.CityName,
                        Destination = uh.Hotel.Destination.CountryName,
                        ImageUrl = uh.Hotel.ImageUrl,
                        Price = uh.Hotel.Price,
                        StartDate = uh.StartDate,
                        EndDate = uh.EndDate
                    })
                    .OrderBy(Uh => Uh.StartDate)
                    .ToListAsync();

            return reservations;
        }
    }
}
