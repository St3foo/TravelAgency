using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.ReservationModels;

namespace TravelAgency.Service.Core
{
    public class ReservationService : IReservationService
    {
        private readonly TravelAgencyDbContext _context;
        private readonly UserManager<IdentityUser> _user;

        public ReservationService(TravelAgencyDbContext context, UserManager<IdentityUser> user)
        {
            _context = context;
            _user = user;
        }

        public async Task<bool> AddReservationAsync(string? userId, AddReservationViewModel? model)
        {
            bool result = false;

            IdentityUser? user = await _user.FindByIdAsync(userId);

            Hotel? hotel = await _context
                .Hotels
                .AsNoTracking()
                .SingleOrDefaultAsync(h => h.Id == model.Id);

            if (user != null && hotel != null)
            {
                UserHotels reservation = new UserHotels 
                {
                    UserId = userId,
                    HotelId = model.Id,
                    StartDate = model.ReservationDate,
                    EndDate = model.ReservationDate.AddDays(model.Nights)
                };

                await _context.UsersHotels.AddAsync(reservation);
                await _context.SaveChangesAsync();

                result = true;
            }

            return result;
        }

        public async Task<IEnumerable<GetAllReservationsViewModel>> GetAllReservationsAsync(string? userId)
        {
            IdentityUser? user = await _user.FindByIdAsync(userId);

            IEnumerable<GetAllReservationsViewModel> reservations = null!;

            if (user != null)
            {
                reservations = await _context
                    .UsersHotels
                    .Include(uh => uh.Hotel)
                    .AsNoTracking()
                    .Where(uh => uh.UserId.ToLower() == userId.ToLower())
                    .Select(uh => new GetAllReservationsViewModel 
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
                    .ToListAsync();
            }

            return reservations;
        }

        public async Task<AddReservationViewModel> GetReservationDetailsAsync(string? hotelId)
        {
            AddReservationViewModel? model = null;

            if (hotelId != null)
            {
                Hotel? hotel = await _context
                    .Hotels
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

        public async Task RemoveFromFavoritesAsync(string? reservationId)
        {
            UserHotels? reservation = await _context
                .UsersHotels
                .SingleOrDefaultAsync(uh => uh.Id.ToString() == reservationId);

            if (reservation != null)
            {
                _context.UsersHotels.Remove(reservation);
                await _context.SaveChangesAsync();
            }
        }
    }
}
