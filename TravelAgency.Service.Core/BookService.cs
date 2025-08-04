using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data.Models;
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

        public async Task<IEnumerable<GetAllBookingsViewModel>> GetAllBookingsAsync()
        {
            IEnumerable<GetAllBookingsViewModel> bookings = await _userTourRepository
                .GetAllAttached()
                .Include(ut => ut.Tour)
                .ThenInclude(t => t.Destination)
                .Include(ut => ut.Tour)
                .ThenInclude(t => t.Hotel)
                .AsNoTracking()
                .Select(t => new GetAllBookingsViewModel
                {
                    Id = t.Id,
                    TourId= t.TourId,
                    Name = t.Tour.Name,
                    ImageUrl = t.Tour.ImageUrl,
                    Price = t.Tour.Price,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    DestinationName = t.Tour.Destination.CountryName,
                    HotelName = t.Tour.Hotel.HotelName,
                    UserName = t.User.NormalizedUserName
                })
                .OrderBy(t => t.StartDate)
                .ToArrayAsync();

            return bookings;
        }

        public async Task<AddBookingViewModel> GetBookingDetailsForAddAsync(string? tourId)
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
                        Id = ut.Id,
                        TourId = ut.TourId,
                        Name = ut.Tour.Name,
                        ImageUrl = ut.Tour.ImageUrl,
                        DestinationName = ut.Tour.Destination.CountryName,
                        HotelName = ut.Tour.Hotel.HotelName,
                        Price = ut.Tour.Price,
                        StartDate = ut.StartDate,
                        EndDate = ut.EndDate,
                    })
                    .OrderBy(ut => ut.StartDate)
                    .ToListAsync();
            }

            return bookings;
        }

        public async Task RemoveBookingAsync(string? id)
        {
            UserTour? booking = await _userTourRepository
                .SingleOrDefaultAsync(ut => ut.Id.ToString().ToLower() == id.ToLower());

            if (booking != null)
            {
                await _userTourRepository.HardDeleteAsync(booking);
            }
        }
    }
}
