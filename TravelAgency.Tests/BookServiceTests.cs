using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;
using TravelAgency.Service.Core;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.Book;
using TravelAgency.ViewModels.Models.DestinationModels;

namespace TravelAgency.Tests
{
    [TestFixture]
    public class BookServiceTests
    {
        private Mock<IUserTourRepository> _userTourRepositoryMock;
        private Mock<ITourRepository> _tourRepositoryMock;
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private BookService _bookService;

        [SetUp]
        public void Setup()
        {
            _userTourRepositoryMock = new Mock<IUserTourRepository>(MockBehavior.Strict);
            _tourRepositoryMock = new Mock<ITourRepository>(MockBehavior.Strict);

            var store = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );

            _bookService = new BookService(_userTourRepositoryMock.Object, _userManagerMock.Object, _tourRepositoryMock.Object);
        }

        [Test]
        public async Task AddBookingAsyncValidInputShouldAddBookingAndReturnTrue()
        {
            var userId = "user123";
            var user = new IdentityUser { Id = userId };

            var model = new AddBookingViewModel
            {
                Id = Guid.NewGuid(),
                BookingDate = DateTime.Today,
                Nights = 3
            };

            var tour = new Tour { Id = model.Id };

            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user);

            _tourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(new List<Tour> { tour }.AsQueryable().BuildMock());

            _userTourRepositoryMock
                .Setup(r => r.AddAsync(It.Is<UserTour>(b =>
                    b.UserId == userId &&
                    b.TourId == model.Id &&
                    b.StartDate == model.BookingDate &&
                    b.EndDate == model.BookingDate.AddDays(model.Nights)
                )))
                .Returns(Task.CompletedTask);

            var result = await _bookService.AddBookingAsync(userId, model);

            Assert.IsTrue(result);
            _userTourRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserTour>()), Times.Once);
        }

        [Test]
        public async Task AddBookingAsyncUserNotFoundShouldReturnFalse()
        {
            var model = new AddBookingViewModel
            {
                Id = Guid.NewGuid(),
                BookingDate = DateTime.Today,
                Nights = 2
            };

            _tourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(new List<Tour>().AsQueryable().BuildMock());

            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((IdentityUser)null!);

            var result = await _bookService.AddBookingAsync("someUser", model);

            Assert.IsFalse(result);
            _userTourRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserTour>()), Times.Never);
        }

        [Test]
        public async Task AddBookingAsyncTourNotFoundShouldReturnFalse()
        {
            var userId = "user123";
            var model = new AddBookingViewModel
            {
                Id = Guid.NewGuid(),
                BookingDate = DateTime.Today,
                Nights = 1
            };

            var user = new IdentityUser { Id = userId };

            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user);

            _tourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(new List<Tour>().AsQueryable().BuildMock());

            var result = await _bookService.AddBookingAsync(userId, model);

            Assert.IsFalse(result);
            _userTourRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserTour>()), Times.Never);
        }

        [Test]
        public async Task AddBookingAsyncModelIsNullShouldReturnFalse()
        {
            _tourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(new List<Tour>().AsQueryable().BuildMock());

            var result = await _bookService.AddBookingAsync("user123", null);

            Assert.IsFalse(result);
            _userTourRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserTour>()), Times.Never);
        }

        [Test]
        public async Task GetAllBookingsAsyncShouldReturnAllBookings()
        {
            var destination = new Destination
            {
                CountryName = "Italy"
            };

            var hotel = new Hotel
            {
                HotelName = "Rome Inn"
            };

            var tour = new Tour
            {
                Id = Guid.NewGuid(),
                Name = "Rome Adventure",
                ImageUrl = "rome.jpg",
                Price = 999,
                Destination = destination,
                Hotel = hotel
            };

            var user = new IdentityUser
            {
                NormalizedUserName = "TESTUSER"
            };

            var booking = new UserTour
            {
                Id = Guid.NewGuid(),
                TourId = tour.Id,
                Tour = tour,
                StartDate = new DateTime(2025, 8, 10),
                EndDate = new DateTime(2025, 8, 15),
                User = user
            };

            var bookings = new List<UserTour> { booking }.AsQueryable().BuildMock();

            _userTourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(bookings);

            var result = (await _bookService.GetAllBookingsAsync()).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].TourId, Is.EqualTo(tour.Id));
            Assert.That(result[0].Name, Is.EqualTo("Rome Adventure"));
            Assert.That(result[0].DestinationName, Is.EqualTo("Italy"));
            Assert.That(result[0].HotelName, Is.EqualTo("Rome Inn"));
            Assert.That(result[0].UserName, Is.EqualTo("TESTUSER"));
        }

        [Test]
        public async Task GetAllBookingsShoudReturnEmptyCollectionWhenNoBookingsAreFound() 
        {
            List<UserTour> emptyList = new List<UserTour>();
            IQueryable<UserTour> emptyQueryable = emptyList.BuildMock();

            _userTourRepositoryMock
                .Setup(d => d.GetAllAttached())
                .Returns(emptyQueryable);

            IEnumerable<GetAllBookingsViewModel> emptyViewModel = await _bookService.GetAllBookingsAsync();

            Assert.IsNotNull(emptyViewModel);
            Assert.That(emptyViewModel.Count(), Is.EqualTo(emptyList.Count()));
        }

        [Test]
        public async Task GetBookingDetailsForAddAsyncWithValidTourIdReturnsCorrectBookingDetails()
        {
            var tourId = Guid.NewGuid();

            var tour = new Tour
            {
                Id = tourId,
                Name = "Bali Paradise",
                Hotel = new Hotel { HotelName = "Bali Resort" },
                Destination = new Destination { CountryName = "Indonesia" },
                ImageUrl = "bali.jpg",
                Price = 1500,
                DaysStay = 5
            };

            var tours = new List<Tour> { tour }.AsQueryable().BuildMock();

            _tourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(tours);

            var result = await _bookService.GetBookingDetailsForAddAsync(tourId.ToString());

            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(tour.Id));
            Assert.That(result.TourName, Is.EqualTo(tour.Name));
            Assert.That(result.HotelName, Is.EqualTo(tour.Hotel.HotelName));
            Assert.That(result.DestinationName, Is.EqualTo(tour.Destination.CountryName));
            Assert.That(result.ImageUrl, Is.EqualTo(tour.ImageUrl));
            Assert.That(result.Price, Is.EqualTo(tour.Price));
            Assert.That(result.Nights, Is.EqualTo(tour.DaysStay));
            Assert.That(result.BookingDate, Is.Not.EqualTo(default(DateTime)));
        }

        [Test]
        public async Task GetBookingDetailsForAddAsyncWithNullTourIdReturnsNull()
        {
            var result = await _bookService.GetBookingDetailsForAddAsync(null);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetBookingDetailsForAddAsyncTourNotFoundReturnsNull()
        {
            var tourId = Guid.NewGuid().ToString();

            var emptyTours = new List<Tour>().AsQueryable().BuildMock();
            _tourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(emptyTours);

            var result = await _bookService.GetBookingDetailsForAddAsync(tourId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserBookingsAsyncUserNotFoundReturnsNull()
        {
            string userId = "invalid-user-id";
            _userManagerMock
                .Setup(u => u.FindByIdAsync(userId))
                .ReturnsAsync((IdentityUser?)null);

            var result = await _bookService.GetUserBookingsAsync(userId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserBookingsAsyncValidUserReturnsBookings()
        {
            string userId = "test-user-id";

            var user = new IdentityUser { Id = userId };
            _userManagerMock
                .Setup(u => u.FindByIdAsync(userId))
                .ReturnsAsync(user);

            var bookingList = new List<UserTour>
                {
                    new UserTour
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        TourId = Guid.NewGuid(),
                        StartDate = DateTime.Today,
                        EndDate = DateTime.Today.AddDays(3),
                        Tour = new Tour
                        {
                            Name = "Adventure Tour",
                            ImageUrl = "url",
                            Price = 999.99m,
                            Destination = new Destination { CountryName = "Italy" },
                            Hotel = new Hotel { HotelName = "Hotel Roma" }
                        }
                    }
                };

            var bookingsQuery = bookingList.AsQueryable().BuildMock();

            _userTourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(bookingsQuery);

            var result = await _bookService.GetUserBookingsAsync(userId);

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Adventure Tour"));
            Assert.That(result.First().DestinationName, Is.EqualTo("Italy"));
        }

        [Test]
        public async Task RemoveBookingAsyncValidIdDeletesBooking()
        {
            string id = Guid.NewGuid().ToString();
            var booking = new UserTour { Id = Guid.Parse(id) };

            _userTourRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<UserTour, bool>>>()))
                .ReturnsAsync(booking);

            _userTourRepositoryMock
                .Setup(r => r.HardDeleteAsync(booking))
                .ReturnsAsync(true)
                .Verifiable();

            await _bookService.RemoveBookingAsync(id);

            _userTourRepositoryMock.Verify(r => r.HardDeleteAsync(booking), Times.Once);
        }

        [Test]
        public async Task RemoveBookingAsyncInvalidIdDoesNotDeleteBooking()
        {
            string id = Guid.NewGuid().ToString();

            _userTourRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<UserTour, bool>>>()))
                .ReturnsAsync((UserTour?)null);

            await _bookService.RemoveBookingAsync(id);

            _userTourRepositoryMock.Verify(r => r.HardDeleteAsync(It.IsAny<UserTour>()), Times.Never);
        }
    }
}
