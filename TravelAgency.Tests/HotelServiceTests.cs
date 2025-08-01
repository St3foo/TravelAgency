using System.Linq.Expressions;
using MockQueryable;
using Moq;
using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;
using TravelAgency.Service.Core;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.HotelModels;
using TravelAgency.ViewModels.Models.LandmarkModels;
using TravelAgency.ViewModels.Models.TourModels;

namespace TravelAgency.Tests
{
    [TestFixture]
    public class HotelServiceTests
    {
        private Mock<IHotelRepository> _hotelRepositoryMock;
        private Mock<IDestinationRepository> _destinationRepositoryMock;
        private IHotelService _hotelService;

        [SetUp]
        public void Setup() 
        {
            _hotelRepositoryMock = new Mock<IHotelRepository>(MockBehavior.Strict);
            _destinationRepositoryMock = new Mock<IDestinationRepository>(MockBehavior.Strict);
            _hotelService = new HotelService(_hotelRepositoryMock.Object, _destinationRepositoryMock.Object);
        }

        [Test]
        public void PassAlways() 
        {
            Assert.Pass();
        }

        [Test]
        public async Task AddHotelWithValidModelAndDestinationReturnsTrue() 
        {
            var destinationId = Guid.NewGuid();
            var model = new AddHotelViewModel
            {
                Name = "Name",
                Description = "Description",
                CityName = "CityName",
                ImageUrl = null,
                Price = 1,
                Nights = 1,
                DestinationId = destinationId.ToString()
            };

            var destination = new Destination { Id = destinationId };
            _destinationRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.Is<Expression<Func<Destination, bool>>>(
                    expr => expr.Compile()(destination)
                        )))
                    .ReturnsAsync(destination);

            _hotelRepositoryMock
                .Setup(r => r.AddAsync(It.Is<Hotel>(h =>
                    h.HotelName == model.Name &&
                    h.Description == model.Description &&
                    h.ImageUrl == model.ImageUrl &&
                    h.CityName == model.CityName &&
                    h.Price == model.Price &&
                    h.DaysStay == model.Nights &&
                    h.DestinationId == destinationId)))
                .Returns(Task.CompletedTask);

            var result = await _hotelService.AddHotelAsync(model);

            Assert.True(result);
        }

        [Test]
        public async Task AddHotelAsyncWithNullModelShouldReturnFalse()
        {
            _destinationRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Destination, bool>>>()))
                .ReturnsAsync((Destination?)null);

            var result = await _hotelService.AddHotelAsync(null);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddHotelAsyncWhenDestinationNotFoundShouldReturnFalse()
        {
            var model = new AddHotelViewModel
            {
                Name = "Unknown Tower",
                Description = "Mystery",
                ImageUrl = null,
                CityName = "Nowhere",
                DestinationId = Guid.NewGuid().ToString()
            };

            _destinationRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Destination, bool>>>()))
                .ReturnsAsync((Destination?)null);

            var result = await _hotelService.AddHotelAsync(model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteOrRestoreLandmarkAsyncWithValidIdShouldToggleIsDeletedAndUpdate()
        {
            var id = Guid.NewGuid();
            var hotel = new Hotel
            {
                Id = id,
                IsDeleted = false
            };

            var hotelList = new List<Hotel> { hotel };

            IQueryable<Hotel> queryList = hotelList.BuildMock();

            _hotelRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(queryList);

            _hotelRepositoryMock
                .Setup(r => r.UpdateAsync(It.Is<Hotel>(l => l.Id == id && l.IsDeleted == true)))
                .ReturnsAsync(true);

            await _hotelService.DeleteOrRestoreHotelAsync(id.ToString());

            Assert.IsTrue(hotel.IsDeleted);
            _hotelRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Hotel>()), Times.Once);
        }

        [Test]
        public async Task DeleteOrRestoreLandmarkAsyncWithNullIdShouldDoNothing()
        {
            await _hotelService.DeleteOrRestoreHotelAsync(null);

            _hotelRepositoryMock.Verify(r => r.GetAllAttached(), Times.Never);
            _hotelRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Hotel>()), Times.Never);
        }

        [Test]
        public async Task DeleteOrRestoreLandmarkAsyncWithEmptyIdShouldDoNothing()
        {
            await _hotelService.DeleteOrRestoreHotelAsync(" ");

            _hotelRepositoryMock.Verify(r => r.GetAllAttached(), Times.Never);
            _hotelRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Hotel>()), Times.Never);
        }

        [Test]
        public async Task DeleteOrRestoreHotelAsyncWithNonMatchingIdShouldDoNothing()
        {

            var differentHotel = new Hotel
            {
                Id = Guid.NewGuid(),
                IsDeleted = false
            };

            var hotelList = new List<Hotel> { differentHotel };
            IQueryable<Hotel> queryList = hotelList.BuildMock();

            _hotelRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(queryList);

            await _hotelService.DeleteOrRestoreHotelAsync(Guid.NewGuid().ToString());

            _hotelRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Hotel>()), Times.Never);
        }

        [Test]
        public async Task GetAllHotelsReturnEmptyListWhenCollectionIsEmpty()
        {
            List<Hotel> hotels = new List<Hotel>();
            IQueryable<Hotel> queryList = hotels.BuildMock();

            _hotelRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            IEnumerable<GetAllHotelsViewModel> allLand = await _hotelService.GetAllHotelsAsync();

            Assert.IsEmpty(allLand);
        }

        [Test]
        public async Task GetAllHotelsReturnList()
        {
            var hotelkId = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1");
            var destinationId = Guid.Parse("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd");

            var destination = new Destination
            {
                Id = destinationId,
                CountryName = "BG"
            };

            var hotel = new Hotel
            {
                Id = hotelkId,
                HotelName = "Name",
                Description = "Description",
                ImageUrl = null,               
                Destination = destination,
                DestinationId = destinationId,
                CityName = "Name",               
            };

            var hotelList = new List<Hotel> { hotel }.BuildMock();

            _hotelRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(hotelList);

            IEnumerable<GetAllHotelsViewModel> allHotels = await _hotelService.GetAllHotelsAsync();

            Assert.IsNotEmpty(allHotels);
            Assert.That(hotelList.Count(), Is.EqualTo(allHotels.Count()));
            foreach (var hot in allHotels)
            {
                GetAllHotelsViewModel hotelVm = allHotels.FirstOrDefault(l => l.Id.ToString() == hot.Id.ToString());

                Assert.IsNotNull(hotelVm);
                Assert.That(hot.Name, Is.EqualTo(hotelVm.Name));
            }
        }

        [Test]
        public async Task GetAllHotelsByDestIdReturnEmptyListWhenThereIsNoMatch()
        {
            List<Hotel> hotels = new List<Hotel>();
            IQueryable<Hotel> queryList = hotels.BuildMock();

            _hotelRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            IEnumerable<GetAllHotelsViewModel> allHotels = await _hotelService.GetAllHotelsByDestinationIdAsync("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd");

            Assert.IsEmpty(allHotels);
        }

        [Test]
        public async Task GetAllHotelsByDestIdReturnLandmarksWhenIdIsCorrect()
        {
            var hotelId = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1");
            var destinationId = Guid.Parse("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd");

            var destination = new Destination
            {
                Id = destinationId,
                CountryName = "BG"
            };

            var hotel = new Hotel
            {
                Id = hotelId,
                HotelName = "Name",
                Description = "Description",
                ImageUrl = null,
                CityName = "Loc",
                Destination = destination,
                DestinationId = destinationId,
            };

            var hotelList = new List<Hotel> { hotel }.BuildMock();

            _hotelRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(hotelList);

            IEnumerable<GetAllHotelsViewModel> allHotels = await _hotelService.GetAllHotelsByDestinationIdAsync(destinationId.ToString());

            Assert.IsNotEmpty(allHotels);
            Assert.That(hotelList.Count(), Is.EqualTo(allHotels.Count()));
            foreach (var hot in allHotels)
            {
                GetAllHotelsViewModel hotVm = allHotels.FirstOrDefault(l => l.Id.ToString() == hot.Id.ToString());

                Assert.IsNotNull(hotVm);
                Assert.That(hot.Name, Is.EqualTo(hotVm.Name));
            }
        }

        [Test]
        public async Task GetAllHotelsForAdminReturnEmptyListWhenTherAreNoHotels()
        {
            List<Hotel> hotels = new List<Hotel>();
            IQueryable<Hotel> queryList = hotels.BuildMock();

            _hotelRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            IEnumerable<GetAllHotelsViewModel> allHotels = await _hotelService.GetAllHotelsForAdminAsync();

            Assert.IsEmpty(allHotels);
        }

        [Test]
        public async Task GetAllHotelsForAdminReturnsAllHotels()
        {
            var hotelId = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1");
            var hotelId1 = Guid.Parse("a4c32ad7-eaa4-4498-9986-24518b4d022a");
            var destinationId = Guid.Parse("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd");

            var destination = new Destination
            {
                Id = destinationId,
                CountryName = "BG"
            };

            var hotel = new Hotel
            {
                Id = hotelId,
                HotelName = "Name",
                Description = "Description",
                ImageUrl = null,
                CityName = "Loc",
                Destination = destination,
                DestinationId = destinationId,
            };
            var hotel1 = new Hotel
            {
                Id = hotelId1,
                HotelName = "Name",
                Description = "Description",
                ImageUrl = null,
                CityName = "Loc",
                Destination = destination,
                DestinationId = destinationId,
                IsDeleted = true
            };


            var hotelList = new List<Hotel> { hotel, hotel1 }.BuildMock();

            _hotelRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(hotelList);

            IEnumerable<GetAllHotelsViewModel> allHotels = await _hotelService.GetAllHotelsForAdminAsync();

            Assert.IsNotEmpty(allHotels);
            Assert.That(hotelList.Count(), Is.EqualTo(allHotels.Count()));
            foreach (var hot in allHotels)
            {
                GetAllHotelsViewModel hotVm = allHotels.FirstOrDefault(l => l.Id.ToString() == hot.Id.ToString());

                Assert.IsNotNull(hotVm);
                Assert.That(hot.Name, Is.EqualTo(hotVm.Name));
            }
        }

        [Test]
        public async Task GetHotelForEditReturnsNullWithNotExistingId()
        {
            List<Hotel> hotels = new List<Hotel>();
            IQueryable<Hotel> queryList = hotels.BuildMock();

            _hotelRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            HotelEditViewModel? model = await _hotelService.GetHotelForEditAsync(Guid.NewGuid().ToString());

            Assert.IsNull(model);
        }

        [Test]
        public async Task GetHotelForEditReturnsExistingHotel()
        {
            Hotel newHotel = new Hotel()
            {
                Id = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1"),
                HotelName = "Name",
                Description = "Description",
                ImageUrl = null,
                CityName = "LocationName",
                DestinationId = Guid.Parse("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd")
            };

            List<Hotel> hotels = new List<Hotel>() { newHotel };
            IQueryable<Hotel> queryList = hotels.BuildMock();

            _hotelRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            HotelEditViewModel? model = await _hotelService.GetHotelForEditAsync("271cf215-ce36-4fc9-87e5-c71e214af3a1");

            Assert.IsNotNull(model);
            Assert.That(newHotel.HotelName, Is.EqualTo(model.Name));
            Assert.That(newHotel.Description, Is.EqualTo(model.Description));
            Assert.That(newHotel.CityName, Is.EqualTo(model.CityName));
        }

        [Test]
        public async Task GetHotelDetailsReturnNullWithWrongId()
        {
            Hotel newHotel = new Hotel()
            {
                Id = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1"),
                HotelName = "Name",
                Description = "Description",
                ImageUrl = null,
                CityName = "LocationName",
                DestinationId = Guid.Parse("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd")
            };

            List<Hotel> hotels = new List<Hotel>() { newHotel };
            IQueryable<Hotel> queryList = hotels.BuildMock();

            _hotelRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            HotelDetailsViewModel? hot = await _hotelService.GetHotelDetailsAsync("a6959478-fc72-432f-9b55-a0b6d33e457e");

            Assert.IsNull(hot);
        }

        [Test]
        public async Task GetHotelDetailsReturnNullWithNullId()
        {
            Hotel newHotel = new Hotel()
            {
                Id = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1"),
                HotelName = "Name",
                Description = "Description",
                ImageUrl = null,
                CityName = "LocationName",
                DestinationId = Guid.Parse("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd")
            };

            List<Hotel> hotels = new List<Hotel>() { newHotel };
            IQueryable<Hotel> queryList = hotels.BuildMock();

            _hotelRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            HotelDetailsViewModel? hot = await _hotelService.GetHotelDetailsAsync(null);

            Assert.IsNull(hot);
        }

        [Test]
        public async Task GetHotelByIdReturnsCorrectHotel()
        {
            var hotelId = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1");
            var destinationId = Guid.Parse("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd");

            var destination = new Destination
            {
                Id = destinationId,
                CountryName = "BG"
            };

            var hotel = new Hotel
            {
                Id = hotelId,
                HotelName = "Name",
                Description = "Description",
                ImageUrl = null,
                CityName = "Loc",
                Destination = destination,
                DestinationId = destinationId,
            };

            var hotelList = new List<Hotel> { hotel }.BuildMock();

            _hotelRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(hotelList);

            HotelDetailsViewModel? hot = await _hotelService.GetHotelDetailsAsync(hotelId.ToString());

            Assert.IsNotNull(hot);
            Assert.That(hot.Id, Is.EqualTo(hotelId.ToString()));
            Assert.That(hot.Title, Is.EqualTo(hotel.HotelName));
            Assert.That(hot.Description, Is.EqualTo(hotel.Description));
        }

        [Test]
        public async Task GetHotelForTourReturnsNullIfIdIsInvalid()
        {
            List<Hotel> hotels = new List<Hotel>();
            IQueryable<Hotel> queryList = hotels.BuildMock();

            _hotelRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            IEnumerable<GetAllHotelsForAddTourViewModel>? model = await _hotelService.GetHotelsForTourAsync(Guid.NewGuid().ToString());

            Assert.IsEmpty(model);
        }

        [Test]
        public async Task GetHotelForTourReturnsHotelWithCorrectId()
        {
            Hotel newHotel = new Hotel()
            {
                Id = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1"),
                HotelName = "Name",
                Description = "Description",
                ImageUrl = null,
                CityName = "LocationName",
                DestinationId = Guid.Parse("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd")
            };

            List<Hotel> hotels = new List<Hotel>() { newHotel };
            IQueryable<Hotel> queryList = hotels.BuildMock();

            _hotelRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            IEnumerable<GetAllHotelsForAddTourViewModel>? model = await _hotelService.GetHotelsForTourAsync("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd");

            Assert.IsNotNull(model);
            Assert.That(model.Count(), Is.EqualTo(hotels.Count()));

            foreach (var hot in model)
            {
                GetAllHotelsForAddTourViewModel? hotVm = model.FirstOrDefault(l => l.Id.ToString() == hot.Id.ToString());

                Assert.IsNotNull(hotVm);
                Assert.That(hot.Name, Is.EqualTo(hotVm.Name));
            }
        }

        [Test]
        public async Task SaveEditChangesAsyncNullModelReturnsFalse()
        {
            var result = await _hotelService.SaveEditChangesAsync(null);

            Assert.IsFalse(result);
            _hotelRepositoryMock.Verify(r => r.GetAllAttached(), Times.Never);
            _hotelRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Hotel>()), Times.Never);
        }

        [Test]
        public async Task SaveEditChangesAsyncValidModelUpdatesHotelAndReturnsTrue()
        {
            var model = new HotelEditViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Updated Name",
                Description = "Updated Description",
                ImageUrl = "image.jpg",
                CityName = "New Location",
                DestinationId = Guid.NewGuid().ToString()
            };

            var hotel = new Hotel { Id = Guid.Parse(model.Id) };
            var destination = new Destination { Id = Guid.Parse(model.DestinationId) };

            var mockList = new List<Hotel> { hotel }.BuildMock();

            _hotelRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockList);

            _destinationRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Destination, bool>>>()))
                .ReturnsAsync(destination);

            _hotelRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Hotel>()))
                .ReturnsAsync(true);

            var result = await _hotelService.SaveEditChangesAsync(model);

            Assert.IsTrue(result);
            Assert.AreEqual(model.Name, hotel.HotelName);
            Assert.AreEqual(model.Description, hotel.Description);
            Assert.AreEqual(model.ImageUrl, hotel.ImageUrl);
            Assert.AreEqual(model.CityName, hotel.CityName);
            Assert.AreEqual(Guid.Parse(model.DestinationId), hotel.DestinationId);
        }

        [Test]
        public async Task SaveEditChangesAsyncHotelNotFoundReturnsFalse()
        {
            var model = new HotelEditViewModel
            {
                Id = Guid.NewGuid().ToString(),
                DestinationId = Guid.NewGuid().ToString()
            };

            var mockList = new List<Hotel>().BuildMock();

            _hotelRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockList);

            _destinationRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Destination, bool>>>()))
                .ReturnsAsync((Destination?)null);

            var result = await _hotelService.SaveEditChangesAsync(model);

            Assert.IsFalse(result);
            _hotelRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Hotel>()), Times.Never);
        }

        [Test]
        public async Task SaveEditChangesAsyncDestinationNotFoundReturnsFalse()
        {
            var model = new HotelEditViewModel
            {
                Id = Guid.NewGuid().ToString(),
                DestinationId = Guid.NewGuid().ToString()
            };

            var hotel = new Hotel { Id = Guid.Parse(model.Id) };
            var mockList = new List<Hotel> { hotel }.BuildMock();

            _hotelRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockList);

            _destinationRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Destination, bool>>>()))
                .ReturnsAsync((Destination?)null);

            var result = await _hotelService.SaveEditChangesAsync(model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SaveEditChangesAsyncUpdateFailsReturnsFalse()
        {
            var model = new HotelEditViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Name",
                Description = "Desc",
                ImageUrl = "url",
                CityName = "loc",
                DestinationId = Guid.NewGuid().ToString()
            };

            var hotel = new Hotel { Id = Guid.Parse(model.Id) };
            var destination = new Destination { Id = Guid.Parse(model.DestinationId) };

            var mockList = new List<Hotel> { hotel }.BuildMock();

            _hotelRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockList);

            _destinationRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Destination, bool>>>()))
                .ReturnsAsync(destination);

            _hotelRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Hotel>()))
                .ReturnsAsync(false);

            var result = await _hotelService.SaveEditChangesAsync(model);

            Assert.IsFalse(result);
        }
    }
}
