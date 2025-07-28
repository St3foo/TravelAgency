using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data.Models;
using TravelAgency.Data.Repository;
using TravelAgency.Data.Repository.Interfaces;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.Book;

namespace TravelAgency.Service.Core
{
    public class BookService : IBookService
    {
        private readonly IUserTourRepository _userTourRepository;
        private readonly ITourRepository _tourRepository;
        private readonly UserManager<IdentityUser> _user;

        public BookService(IUserTourRepository userTourRepository, UserManager<IdentityUser> user, ITourRepository tourRepository)
        {
            _userTourRepository = userTourRepository;
            _user = user;
            _tourRepository = tourRepository;
        }

        public async Task<bool> AddBookingAsync(string? userId, AddBookingViewModel? model)
        {
            bool result = false;

            IdentityUser? user = await _user.FindByIdAsync(userId);

            Tour? tour = await _tourRepository
                .GetAllAttached()
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id == model.Id);

            if (user != null && tour != null)
            {
                UserTour booking = new UserTour
                {
                    UserId = userId,
                    TourId = model.Id,
                    StartDate = model.BookingDate,
                    EndDate = model.BookingDate.AddDays(model.Nights)
                };

                await _userTourRepository.AddAsync(booking);

                result = true;
            }

            return result;
        }

        public async Task<AddBookingViewModel> GetBookingDetailsAsync(string? tourId)
        {
            AddBookingViewModel? model = null;

            if (tourId != null)
            {
                Tour? tour = await _tourRepository
                    .GetAllAttached()
                    .Include(t => t.Hotel)
                    .Include(t => t.Destination)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(t => t.Id.ToString().ToLower() == tourId.ToLower());

                if (tour != null)
                {
                    model = new AddBookingViewModel
                    {
                        Id = tour.Id,
                        TourName = tour.Name,
                        HotelName = tour.Hotel.HotelName,
                        DestinationName = tour.Destination.CountryName,
                        ImageUrl = tour.ImageUrl,
                        Price = tour.Price,
                        Nights = tour.DaysStay,
                        BookingDate = DateTime.Now
                    };
                }
            }

            return model;
        }

        public async Task<IEnumerable<GetUserBookingsViewModel>> GetUserBookingsAsync(string? userId)
        {
            IdentityUser? user = await _user.FindByIdAsync(userId);

            IEnumerable<GetUserBookingsViewModel> bookings = null!;

            if (user != null)
            {
                bookings = await _userTourRepository
                    .GetAllAttached()
                    .Include(ut => ut.Tour)
                    .ThenInclude(t => t.Destination)
                    .Include(ut => ut.Tour)
                    .ThenInclude(t => t.Hotel)
                    .AsNoTracking()
                    .Where(ut => ut.UserId.ToLower() == userId.ToLower())
                    .Select(ut => new GetUserBookingsViewModel
                    {
                        TourId = ut.TourId,
                        Name = ut.Tour.Name,
                        ImageUrl = ut.Tour.ImageUrl,
                        DestinationName = ut.Tour.Destination.CountryName,
                        HotelName = ut.Tour.Hotel.HotelName,
                        Price = ut.Tour.Price,
                        StartDate = ut.StartDate,
                        EndDate = ut.EndDate,
                    })
                    .ToListAsync();
            }

            return bookings;
        }

        public async Task RemoveBookingAsync(string? id)
        {
            UserTour? booking = await _userTourRepository
                .SingleOrDefaultAsync(ut => ut.TourId.ToString().ToLower() == id.ToLower());

            if (booking != null)
            {
                await _userTourRepository.HardDeleteAsync(booking);
            }
        }
    }
}
