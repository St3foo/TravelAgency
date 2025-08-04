using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;
using TravelAgency.Service.Core;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.Book;
using TravelAgency.ViewModels.Models.ReservationModels;

namespace TravelAgency.Tests
{
    [TestFixture]
    public class ReservationServiceTests
    {
        private Mock<IUserHotelRepository> _userHotelRepositoryMock;
        private Mock<IHotelRepository> _hotelRepositoryMock;
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private ReservationService _reservationService;

        [SetUp]
        public void Setup() 
        {
            _userHotelRepositoryMock = new Mock<IUserHotelRepository>();
            _hotelRepositoryMock = new Mock<IHotelRepository>();

            var store = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );

            _reservationService = new ReservationService(_userHotelRepositoryMock.Object, _hotelRepositoryMock.Object, _userManagerMock.Object );
        }

        [Test]
        public async Task AddReservationAsyncValidInputShouldAddReservationAndReturnTrue()
        {
            var userId = "user123";
            var user = new IdentityUser { Id = userId };

            var model = new AddReservationViewModel
            {
                Id = Guid.NewGuid(),
                ReservationDate = DateTime.Now,
                Nights = 3
            };

            var hotel = new Hotel { Id = model.Id };

            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user);

            _hotelRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(new List<Hotel> { hotel }.AsQueryable().BuildMock());

            _userHotelRepositoryMock
                .Setup(r => r.AddAsync(It.Is<UserHotel>(b =>
                    b.UserId == userId &&
                    b.HotelId == model.Id &&
                    b.StartDate == model.ReservationDate &&
                    b.EndDate == model.ReservationDate.AddDays(model.Nights)
                )))
                .Returns(Task.CompletedTask);

            var result = await _reservationService.AddReservationAsync(userId, model);

            Assert.IsTrue(result);
            _userHotelRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserHotel>()), Times.Once);
        }

        [Test]
        public async Task AddReservationAsyncUserNotFoundShouldReturnFalse()
        {
            var model = new AddReservationViewModel
            {
                Id = Guid.NewGuid(),
                ReservationDate = DateTime.Today,
                Nights = 2
            };

            _hotelRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(new List<Hotel>().AsQueryable().BuildMock());

            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((IdentityUser)null!);

            var result = await _reservationService.AddReservationAsync("someUser", model);

            Assert.IsFalse(result);
            _userHotelRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserHotel>()), Times.Never);
        }

        [Test]
        public async Task AddReservationAsyncHotelNotFoundShouldReturnFalse()
        {
            var userId = "user123";
            var model = new AddReservationViewModel
            {
                Id = Guid.NewGuid(),
                ReservationDate = DateTime.Today,
                Nights = 1
            };

            var user = new IdentityUser { Id = userId };

            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user);

            _hotelRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(new List<Hotel>().AsQueryable().BuildMock());

            var result = await _reservationService.AddReservationAsync(userId, model);

            Assert.IsFalse(result);
            _userHotelRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserHotel>()), Times.Never);
        }

        [Test]
        public async Task AddReservationAsyncModelIsNullShouldReturnFalse()
        {
            _hotelRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(new List<Hotel>().AsQueryable().BuildMock());

            var result = await _reservationService.AddReservationAsync("user123", null);

            Assert.IsFalse(result);
            _userHotelRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserHotel>()), Times.Never);
        }

        [Test]
        public async Task GetAllReservationAsyncShouldReturnAllReservation()
        {
            var reservations = new List<UserHotel>
            {
                    new UserHotel
                    {
                        Id = Guid.NewGuid(),
                        User = new IdentityUser { NormalizedUserName = "TESTUSER" },
                        HotelId = Guid.NewGuid(),
                        Hotel = new Hotel
                        {
                            HotelName = "Grand Hotel",
                            CityName = "Paris",
                            Destination = new Destination { CountryName = "France" },
                            ImageUrl = "hotel.jpg",
                            Price = 200
                        },
                    StartDate = new DateTime(2025, 10, 1),
                    EndDate = new DateTime(2025, 10, 5)
                    }
            };

            var mockQueryable = reservations.AsQueryable().BuildMock();

            _userHotelRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockQueryable);

            var result = await _reservationService.GetAllReservationsAsync();

            var reservationList = result.ToList();
            Assert.That(reservationList.Count, Is.EqualTo(1));
            Assert.That(reservationList[0].HotelName, Is.EqualTo("Grand Hotel"));
            Assert.That(reservationList[0].UserName, Is.EqualTo("TESTUSER"));
            Assert.That(reservationList[0].Location, Is.EqualTo("Paris"));
            Assert.That(reservationList[0].Destination, Is.EqualTo("France"));
        }

        [Test]
        public async Task GetAllReservationShoudReturnEmptyCollectionWhenNoReservationsAreFound()
        {
            List<UserHotel> emptyList = new List<UserHotel>();
            IQueryable<UserHotel> emptyQueryable = emptyList.BuildMock();

            _userHotelRepositoryMock
                .Setup(d => d.GetAllAttached())
                .Returns(emptyQueryable);

            IEnumerable<GetAllReservationViewModel> emptyViewModel = await _reservationService.GetAllReservationsAsync();

            Assert.IsNotNull(emptyViewModel);
            Assert.That(emptyViewModel.Count(), Is.EqualTo(emptyList.Count()));
        }

        [Test]
        public async Task GetReservationDetailsForAddAsyncWithValidHotelIdReturnsCorrectReservationDetails()
        {
            var hotelId = Guid.NewGuid();

            var hotel = new Hotel
            {
                Id = hotelId,
                HotelName = "Bali Paradise",
                Destination = new Destination { CountryName = "Indonesia" },
                ImageUrl = "bali.jpg",
                Price = 1500,
                DaysStay = 5
            };

            var hotels = new List<Hotel> { hotel }.AsQueryable().BuildMock();

            _hotelRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(hotels);

            var result = await _reservationService.GetReservationDetailsForAddAsync(hotelId.ToString());

            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(hotel.Id));
            Assert.That(result.Name, Is.EqualTo(hotel.HotelName));
            Assert.That(result.Destination, Is.EqualTo(hotel.Destination.CountryName));
            Assert.That(result.ImageUrl, Is.EqualTo(hotel.ImageUrl));
            Assert.That(result.Price, Is.EqualTo(hotel.Price));
            Assert.That(result.Nights, Is.EqualTo(hotel.DaysStay));
            Assert.That(result.ReservationDate, Is.Not.EqualTo(default(DateTime)));
        }

        [Test]
        public async Task GetReservationDetailsForAddAsyncWithNullHotelIdReturnsNull()
        {
            var result = await _reservationService.GetReservationDetailsForAddAsync(null);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetReservationDetailsForAddAsyncHotelNotFoundReturnsNull()
        {
            var hotelId = Guid.NewGuid().ToString();

            var emptyHotels = new List<Hotel>().AsQueryable().BuildMock();
            _hotelRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(emptyHotels);

            var result = await _reservationService.GetReservationDetailsForAddAsync(hotelId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserReservationAsyncUserNotFoundReturnsNull()
        {
            string userId = "invalid-user-id";
            _userManagerMock
                .Setup(u => u.FindByIdAsync(userId))
                .ReturnsAsync((IdentityUser?)null);

            var result = await _reservationService.GetUserReservationsAsync(userId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserReservationsAsyncValidUserReturnsReservations()
        {
            string userId = "user123";

            var identityUser = new IdentityUser
            {
                Id = userId,
                NormalizedUserName = "USER123"
            };

            var reservations = new List<UserHotel>
            {
                    new UserHotel
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        HotelId = Guid.NewGuid(),
                        Hotel = new Hotel
                        {
                            HotelName = "Sunset Resort",
                            CityName = "Barcelona",
                            Destination = new Destination { CountryName = "Spain" },
                            ImageUrl = "image.jpg",
                            Price = 350
                        },
                        StartDate = new DateTime(2025, 9, 10),
                        EndDate = new DateTime(2025, 9, 15)
                    }
            };

            var mockQueryable = reservations.AsQueryable().BuildMock();

            _userManagerMock
                .Setup(u => u.FindByIdAsync(userId))
                .ReturnsAsync(identityUser);

            _userHotelRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockQueryable);

            var result = await _reservationService.GetUserReservationsAsync(userId);

            var list = result.ToList();
            Assert.That(list, Has.Count.EqualTo(1));
            Assert.That(list[0].HotelName, Is.EqualTo("Sunset Resort"));
            Assert.That(list[0].Location, Is.EqualTo("Barcelona"));
            Assert.That(list[0].Destination, Is.EqualTo("Spain"));
        }

        [Test]
        public async Task RemoveReservationAsyncValidIdDeletesReservation()
        {
            string id = Guid.NewGuid().ToString();
            var reservation = new UserHotel { Id = Guid.Parse(id) };

            _userHotelRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<UserHotel, bool>>>()))
                .ReturnsAsync(reservation);

            _userHotelRepositoryMock
                .Setup(r => r.HardDeleteAsync(reservation))
                .ReturnsAsync(true)
                .Verifiable();

            await _reservationService.RemoveFromReservationAsync(id);

            _userHotelRepositoryMock.Verify(r => r.HardDeleteAsync(reservation), Times.Once);
        }

        [Test]
        public async Task RemoveReservationAsyncInvalidIdDoesNotDeleteReservation()
        {
            string id = Guid.NewGuid().ToString();

            _userHotelRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<UserHotel, bool>>>()))
                .ReturnsAsync((UserHotel?)null);

            await _reservationService.RemoveFromReservationAsync(id);

            _userHotelRepositoryMock.Verify(r => r.HardDeleteAsync(It.IsAny<UserHotel>()), Times.Never);
        }
    }
}
