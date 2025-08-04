using MockQueryable.Moq;
using Moq;
using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;
using TravelAgency.Service.Core;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.HotelModels;
using TravelAgency.ViewModels.Models.TourModels;

namespace TravelAgency.Tests
{
    [TestFixture]
    public class TourServiceTests
    {
        private Mock<ITourRepository> _tourRepositoryMock;
        private Mock<ITourLandmarkRepository> _tourLandmarRepositoryMock;
        private TourService _tourService;

        [SetUp]
        public void Setup() 
        {
            _tourLandmarRepositoryMock = new Mock<ITourLandmarkRepository>();
            _tourRepositoryMock = new Mock<ITourRepository>();

            _tourService = new TourService(_tourRepositoryMock.Object, _tourLandmarRepositoryMock.Object);
        }

        [Test]
        public async Task AddTourAsyncValidModelReturnsTrueAndAddsTour()
        {
            var model = new AddTourViewModel
            {
                Name = "Test Tour",
                ImageUrl = "test.jpg",
                Description = "Some description",
                Price = 300m,
                DaysStay = 5,
                DestinationId = Guid.NewGuid(),
                HotelId = Guid.NewGuid(),
                Landmarks = new Guid[] { Guid.NewGuid(), Guid.NewGuid() }
            };

            Tour? capturedTour = null;

            _tourRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Tour>()))
                .Callback<Tour>(tour => capturedTour = tour)
                .Returns(Task.CompletedTask);

            var result = await _tourService.AddTourAsync(model);

            Assert.IsTrue(result);
            Assert.IsNotNull(capturedTour);
            Assert.That(capturedTour.Name, Is.EqualTo(model.Name));
            Assert.That(capturedTour.TourLandmarks.Count, Is.EqualTo(model.Landmarks.Length));
            Assert.That(capturedTour.TourLandmarks.Select(x => x.LandmarkId), Is.EquivalentTo(model.Landmarks));

            _tourRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Tour>()), Times.Once);
        }

        [Test]
        public async Task AddTourAsyncNullModelReturnsFalseAndDoesNotAdd()
        {
            AddTourViewModel? model = null;

            var result = await _tourService.AddTourAsync(model);

            Assert.IsFalse(result);
            _tourRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Tour>()), Times.Never);
        }

        [Test]
        public async Task DeleteOrRestoreTourAsyncWithValidIdShouldToggleIsDeletedAndUpdate()
        {
            var id = Guid.NewGuid();
            var tour = new Tour
            {
                Id = id,
                IsDeleted = false
            };

            var tourList = new List<Tour> { tour };

            IQueryable<Tour> queryList = tourList.BuildMock();

            _tourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(queryList);

            _tourRepositoryMock
                .Setup(r => r.UpdateAsync(It.Is<Tour>(l => l.Id == id && l.IsDeleted == true)))
                .ReturnsAsync(true);

            await _tourService.DeleteOrRestoreTourAsync(id.ToString());

            Assert.IsTrue(tour.IsDeleted);
            _tourRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Tour>()), Times.Once);
        }

        [Test]
        public async Task DeleteOrRestoreTourAsyncWithNullIdShouldDoNothing()
        {
            await _tourService.DeleteOrRestoreTourAsync(null);

            _tourRepositoryMock.Verify(r => r.GetAllAttached(), Times.Never);
            _tourRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Tour>()), Times.Never);
        }

        [Test]
        public async Task DeleteOrRestoreTourAsyncWithEmptyIdShouldDoNothing()
        {
            await _tourService.DeleteOrRestoreTourAsync(" ");

            _tourRepositoryMock.Verify(r => r.GetAllAttached(), Times.Never);
            _tourRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Tour>()), Times.Never);
        }

        [Test]
        public async Task DeleteOrRestoreTourAsyncWithNonMatchingIdShouldDoNothing()
        {

            var differentTour = new Tour
            {
                Id = Guid.NewGuid(),
                IsDeleted = false
            };

            var tourList = new List<Tour> { differentTour };
            IQueryable<Tour> queryList = tourList.BuildMock();

            _tourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(queryList);

            await _tourService.DeleteOrRestoreTourAsync(Guid.NewGuid().ToString());

            _tourRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Tour>()), Times.Never);
        }

        [Test]
        public async Task GetAllToursReturnEmptyListWhenCollectionIsEmpty()
        {
            List<Tour> tour = new List<Tour>();
            IQueryable<Tour> queryList = tour.BuildMock();

            _tourRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            IEnumerable<GetAllToursViewModel> allTours = await _tourService.GetAllToursAsync(null);

            Assert.IsEmpty(allTours);
        }

        [Test]
        public async Task GetAllToursReturnList()
        {
            var tours = new List<Tour>
            {
                new Tour
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Tour 1",
                    Destination = new Destination { CountryName = "Italy" },
                    Hotel = new Hotel { HotelName = "Hotel Roma" },
                    IsDeleted = false,
                    ImageUrl = "image1.jpg",
                    DaysStay = 5,
                    Price = 1000
                },
                new Tour
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Tour 2",
                    Destination = new Destination { CountryName = "France" },
                    Hotel = new Hotel { HotelName = "Hotel Paris" },
                    IsDeleted = true,
                    ImageUrl = "image2.jpg",
                    DaysStay = 7,
                    Price = 1500
                }
            }.AsQueryable();

            var mockTours = tours.BuildMock();

            _tourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockTours);

            var result = await _tourService.GetAllToursAsync(null);

            Assert.IsNotNull(result);
            var list = result.ToList();
            Assert.AreEqual(2, list.Count);

            Assert.That(list[0].Name, Is.EqualTo("Test Tour 1"));
            Assert.That(list[1].HotelName, Is.EqualTo("Hotel Paris"));

            _tourRepositoryMock.Verify(r => r.GetAllAttached(), Times.Once);
        }

        [Test]
        public async Task GetAllToursReturnToursContainingTourWord()
        {
            var tours = new List<Tour>
            {
                new Tour
                {
                    Id = Guid.NewGuid(),
                    Name = "Bulgaria",
                    Destination = new Destination { CountryName = "Italy" },
                    Hotel = new Hotel { HotelName = "Hotel Roma" },
                    IsDeleted = false,
                    ImageUrl = "image1.jpg",
                    DaysStay = 5,
                    Price = 1000
                },
                new Tour
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Tour 2",
                    Destination = new Destination { CountryName = "France" },
                    Hotel = new Hotel { HotelName = "Hotel Paris" },
                    IsDeleted = true,
                    ImageUrl = "image2.jpg",
                    DaysStay = 7,
                    Price = 1500
                }
            }.AsQueryable();

            var mockTours = tours.BuildMock();

            _tourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockTours);

            var result = await _tourService.GetAllToursAsync("Bul");

            Assert.IsNotNull(result);
            var list = result.ToList();
            Assert.AreEqual(1, list.Count);

            Assert.That(list[0].Name, Is.EqualTo("Bulgaria"));

            _tourRepositoryMock.Verify(r => r.GetAllAttached(), Times.Once);
        }

        [Test]
        public async Task GetAllToursReturnEmptyCollectionWhenTourWordDontMatch()
        {
            var tours = new List<Tour>
            {
                new Tour
                {
                    Id = Guid.NewGuid(),
                    Name = "Bulgaria",
                    Destination = new Destination { CountryName = "Italy" },
                    Hotel = new Hotel { HotelName = "Hotel Roma" },
                    IsDeleted = false,
                    ImageUrl = "image1.jpg",
                    DaysStay = 5,
                    Price = 1000
                },
            }.AsQueryable();

            var mockTours = tours.BuildMock();

            _tourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockTours);

            var result = await _tourService.GetAllToursAsync("A");

            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllToursForAdminReturnEmptyListWhenCollectionIsEmpty()
        {
            List<Tour> tour = new List<Tour>();
            IQueryable<Tour> queryList = tour.BuildMock();

            _tourRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            IEnumerable<GetAllToursViewModel> allTours = await _tourService.GetAllToursForAdminAsync(null);

            Assert.IsEmpty(allTours);
        }

        [Test]
        public async Task GetAllToursForAdminReturnList()
        {
            var tours = new List<Tour>
            {
                new Tour
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Tour 1",
                    Destination = new Destination { CountryName = "Italy" },
                    Hotel = new Hotel { HotelName = "Hotel Roma" },
                    IsDeleted = false,
                    ImageUrl = "image1.jpg",
                    DaysStay = 5,
                    Price = 1000
                },
                new Tour
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Tour 2",
                    Destination = new Destination { CountryName = "France" },
                    Hotel = new Hotel { HotelName = "Hotel Paris" },
                    IsDeleted = true,
                    ImageUrl = "image2.jpg",
                    DaysStay = 7,
                    Price = 1500
                }
            }.AsQueryable();

            var mockTours = tours.BuildMock();

            _tourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockTours);

            var result = await _tourService.GetAllToursForAdminAsync(null);

            Assert.IsNotNull(result);
            var list = result.ToList();
            Assert.AreEqual(2, list.Count);

            Assert.That(list[0].Name, Is.EqualTo("Test Tour 1"));
            Assert.That(list[1].HotelName, Is.EqualTo("Hotel Paris"));

            _tourRepositoryMock.Verify(r => r.GetAllAttached(), Times.Once);
        }

        [Test]
        public async Task GetAllToursForAdminReturnToursContainingTourWord()
        {
            var tours = new List<Tour>
            {
                new Tour
                {
                    Id = Guid.NewGuid(),
                    Name = "Bulgaria",
                    Destination = new Destination { CountryName = "Italy" },
                    Hotel = new Hotel { HotelName = "Hotel Roma" },
                    IsDeleted = false,
                    ImageUrl = "image1.jpg",
                    DaysStay = 5,
                    Price = 1000
                },
                new Tour
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Tour 2",
                    Destination = new Destination { CountryName = "France" },
                    Hotel = new Hotel { HotelName = "Hotel Paris" },
                    IsDeleted = true,
                    ImageUrl = "image2.jpg",
                    DaysStay = 7,
                    Price = 1500
                }
            }.AsQueryable();

            var mockTours = tours.BuildMock();

            _tourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockTours);

            var result = await _tourService.GetAllToursForAdminAsync("Bul");

            Assert.IsNotNull(result);
            var list = result.ToList();
            Assert.AreEqual(1, list.Count);

            Assert.That(list[0].Name, Is.EqualTo("Bulgaria"));

            _tourRepositoryMock.Verify(r => r.GetAllAttached(), Times.Once);
        }

        [Test]
        public async Task GetAllToursForAdminReturnEmptyCollectionWhenTourWordDontMatch()
        {
            var tours = new List<Tour>
            {
                new Tour
                {
                    Id = Guid.NewGuid(),
                    Name = "Bulgaria",
                    Destination = new Destination { CountryName = "Italy" },
                    Hotel = new Hotel { HotelName = "Hotel Roma" },
                    IsDeleted = false,
                    ImageUrl = "image1.jpg",
                    DaysStay = 5,
                    Price = 1000
                },
            }.AsQueryable();

            var mockTours = tours.BuildMock();

            _tourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockTours);

            var result = await _tourService.GetAllToursForAdminAsync("A");

            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetTourDetailsReturnNullWithWrongId()
        {
            Tour newTour = new Tour
            {
                Id = Guid.NewGuid()
            };

            List<Tour> tours = new List<Tour>() { newTour };
            IQueryable<Tour> queryList = tours.BuildMock();

            _tourRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            TourDetailsViewModel? tour = await _tourService.GetTourDetailsAsync("a6959478-fc72-432f-9b55-a0b6d33e457e");

            Assert.IsNull(tour);
        }

        [Test]
        public async Task GetTourDetailsReturnNullWithNullId()
        {
            Tour newTour = new Tour
            {
                Id = Guid.NewGuid()
            };

            List<Tour> tours = new List<Tour>() { newTour };
            IQueryable<Tour> queryList = tours.BuildMock();

            _tourRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            TourDetailsViewModel? tour = await _tourService.GetTourDetailsAsync(null);

            Assert.IsNull(tour);
        }

        [Test]
        public async Task GetTourByIdReturnsCorrectTour()
        {
            var tourId = Guid.NewGuid();
            var newTour = new Tour
            {
                    Id = tourId,
                    Name = "Test Tour 1",
                    Destination = new Destination { CountryName = "Italy" },
                    Hotel = new Hotel { HotelName = "Hotel Roma" },
                    IsDeleted = false,
                    ImageUrl = "image1.jpg",
                    DaysStay = 5,
                    Price = 1000
             };

            List<Tour> tours = new List<Tour>() { newTour };
            IQueryable<Tour> queryList = tours.BuildMock();

            _tourRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(queryList);

            var tour = await _tourService.GetTourDetailsAsync(tourId.ToString());

            Assert.IsNotNull(tour);
            Assert.That(tour.Id, Is.EqualTo(tourId.ToString()));
            Assert.That(tour.Name, Is.EqualTo(newTour.Name));
            Assert.That(tour.DestinationName, Is.EqualTo(newTour.Destination.CountryName));
        }

        [Test]
        public async Task SaveEditChangesAsyncModelIsNullReturnsFalse()
        {
            var result = await _tourService.SaveEditChangesAsync(null);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SaveEditChangesAsyncTourNotFoundReturnsFalse()
        {
            var model = new TourEditViewModel { Id = Guid.NewGuid() };

            var queryable = new List<Tour>().AsQueryable().BuildMock();
            _tourRepositoryMock.Setup(r => r.GetAllAttached()).Returns(queryable);

            var result = await _tourService.SaveEditChangesAsync(model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SaveEditChangesAsync_ValidModel_UpdatesTourAndReturnsTrue()
        {
            var tourId = Guid.NewGuid();
            var oldTourLandmarks = new List<TourLandmark>
                {
                    new TourLandmark { TourId = tourId, LandmarkId = Guid.NewGuid() }
                };

            var tour = new Tour
            {
                Id = tourId,
                TourLandmarks = oldTourLandmarks,
            };

            var model = new TourEditViewModel
            {
                Id = tourId,
                Name = "Updated Name",
                Description = "Updated Description",
                ImageUrl = "updated.jpg",
                Price = 300,
                DaysStay = 4,
                HotelId = Guid.NewGuid(),
                DestinationId = Guid.NewGuid(),
                Landmarks = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            var queryable = new List<Tour> { tour }.AsQueryable().BuildMock();
            _tourRepositoryMock.Setup(r => r.GetAllAttached()).Returns(queryable);
            _tourLandmarRepositoryMock.Setup(r => r.HardDeleteAsync(It.IsAny<TourLandmark>())).ReturnsAsync(true);
            _tourRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Tour>())).ReturnsAsync(true);

            var result = await _tourService.SaveEditChangesAsync(model);

            Assert.IsTrue(result);
            _tourLandmarRepositoryMock.Verify(r => r.HardDeleteAsync(It.IsAny<TourLandmark>()), Times.Exactly(oldTourLandmarks.Count));
            _tourRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Tour>(t =>
                t.Name == model.Name &&
                t.TourLandmarks.Count == model.Landmarks.Count()
            )), Times.Once);
        }

        [Test]
        public async Task SaveEditChangesAsyncUpdateFailsReturnsFalse()
        {
            var tourId = Guid.NewGuid();
            var tour = new Tour
            {
                Id = tourId,
                TourLandmarks = new List<TourLandmark>()
            };

            var model = new TourEditViewModel
            {
                Id = tourId,
                Name = "Test",
                Description = "Test Desc",
                ImageUrl = "test.jpg",
                Price = 200,
                DaysStay = 3,
                HotelId = Guid.NewGuid(),
                DestinationId = Guid.NewGuid(),
                Landmarks = new List<Guid> { Guid.NewGuid() }
            };

            var queryable = new List<Tour> { tour }.AsQueryable().BuildMock();
            _tourRepositoryMock.Setup(r => r.GetAllAttached()).Returns(queryable);
            _tourLandmarRepositoryMock.Setup(r => r.HardDeleteAsync(It.IsAny<TourLandmark>())).ReturnsAsync(true);
            _tourRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Tour>())).ReturnsAsync(false);

            var result = await _tourService.SaveEditChangesAsync(model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetTourForEditReturnsNullWithNotExistingId()
        {
            List<Tour> tours = new List<Tour>();
            IQueryable<Tour> queryList = tours.BuildMock();

            _tourRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            TourEditViewModel? model = await _tourService.GetTourForEditAsync(Guid.NewGuid().ToString());

            Assert.IsNull(model);
        }

        [Test]
        public async Task GetTourForEditReturnsExistingTour()
        {
            var tourId = Guid.NewGuid();
            var newTour = new Tour
            {
                Id = tourId,
                Name = "Test Tour 1",
                Destination = new Destination { CountryName = "Italy" },
                Hotel = new Hotel { HotelName = "Hotel Roma" },
                IsDeleted = false,
                ImageUrl = "image1.jpg",
                DaysStay = 5,
                Price = 1000
            };

            List<Tour> tours = new List<Tour>() { newTour };
            IQueryable<Tour> queryList = tours.BuildMock();

            _tourRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            TourEditViewModel? model = await _tourService.GetTourForEditAsync(tourId.ToString());

            Assert.IsNotNull(model);
            Assert.That(newTour.Name, Is.EqualTo(model.Name));
            Assert.That(newTour.ImageUrl, Is.EqualTo(model.ImageUrl));
            Assert.That(newTour.Price, Is.EqualTo(model.Price));
        }
    }
}
