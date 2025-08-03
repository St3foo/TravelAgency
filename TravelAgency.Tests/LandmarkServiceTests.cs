using System.Linq.Expressions;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;
using TravelAgency.Service.Core;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.LandmarkModels;
using TravelAgency.ViewModels.Models.TourModels;

namespace TravelAgency.Tests
{
    [TestFixture]
    public class LandmarkServiceTests
    {
        private Mock<ILandmarkRepository> _landmarkRepositoryMock;
        private Mock<IDestinationRepository> _destinationRepositoryMock;
        private ILandmarkService _landmarkService;

        [SetUp]
        public void Setup()
        {
            _landmarkRepositoryMock = new Mock<ILandmarkRepository>(MockBehavior.Strict);
            _destinationRepositoryMock = new Mock<IDestinationRepository>(MockBehavior.Strict);
            _landmarkService = new LandmarkService(_landmarkRepositoryMock.Object, _destinationRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            Assert.Pass();
        }

        [Test]
        public async Task AddLandmarkWithValidModelAndDestinationReturnsTrue()
        {
            var destinationId = Guid.NewGuid();
            var model = new AddLandmarkViewModel
            {
                Name = "Eiffel Tower",
                Description = "Famous landmark in Paris",
                ImageUrl = null,
                Location = "Paris",
                DestinationId = destinationId.ToString()
            };

            var destination = new Destination { Id = destinationId };

            _destinationRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.Is<Expression<Func<Destination, bool>>>(
                    expr => expr.Compile()(destination)
                        )))
                    .ReturnsAsync(destination);

            _landmarkRepositoryMock
                .Setup(r => r.AddAsync(It.Is<Landmark>(l =>
                    l.Name == model.Name &&
                    l.Description == model.Description &&
                    l.ImageUrl == model.ImageUrl &&
                    l.LocationName == model.Location &&
                    l.DestinationId == destinationId)))
                .Returns(Task.CompletedTask);

            var result = await _landmarkService.AddLandmarkAsync(model);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddLandmarkAsyncWithNullModelShouldReturnFalse()
        {
            var result = await _landmarkService.AddLandmarkAsync(null);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddLandmarkAsyncWhenDestinationNotFoundShouldReturnFalse()
        {
            var model = new AddLandmarkViewModel
            {
                Name = "Unknown Tower",
                Description = "Mystery",
                ImageUrl = null,
                Location = "Nowhere",
                DestinationId = Guid.NewGuid().ToString()
            };

            _destinationRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Destination, bool>>>()))
                .ReturnsAsync((Destination?)null);

            var result = await _landmarkService.AddLandmarkAsync(model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteOrRestoreLandmarkAsyncWithValidIdShouldToggleIsDeletedAndUpdate()
        {
            var id = Guid.NewGuid();
            var landmark = new Landmark
            {
                Id = id,
                IsDeleted = false
            };

            var landmarkList = new List<Landmark> { landmark };

            IQueryable<Landmark> queryList = landmarkList.BuildMock();

            _landmarkRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(queryList);

            _landmarkRepositoryMock
                .Setup(r => r.UpdateAsync(It.Is<Landmark>(l => l.Id == id && l.IsDeleted == true)))
                .ReturnsAsync(true);

            await _landmarkService.DeleteOrRestoreLandmarkAsync(id.ToString());

            Assert.IsTrue(landmark.IsDeleted);
            _landmarkRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Landmark>()), Times.Once);
        }

        [Test]
        public async Task DeleteOrRestoreLandmarkAsyncWithNullIdShouldDoNothing()
        {
            await _landmarkService.DeleteOrRestoreLandmarkAsync(null);

            _landmarkRepositoryMock.Verify(r => r.GetAllAttached(), Times.Never);
            _landmarkRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Landmark>()), Times.Never);
        }

        [Test]
        public async Task DeleteOrRestoreLandmarkAsyncWithEmptyIdShouldDoNothing()
        {
            await _landmarkService.DeleteOrRestoreLandmarkAsync(" ");

            _landmarkRepositoryMock.Verify(r => r.GetAllAttached(), Times.Never);
            _landmarkRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Landmark>()), Times.Never);
        }

        [Test]
        public async Task DeleteOrRestoreLandmarkAsyncWithNonMatchingIdShouldDoNothing()
        {

            var differentLandmark = new Landmark
            {
                Id = Guid.NewGuid(),
                IsDeleted = false
            };

            var landmarkList = new List<Landmark> { differentLandmark };
            IQueryable<Landmark> queryList = landmarkList.BuildMock();

            _landmarkRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(queryList);

            await _landmarkService.DeleteOrRestoreLandmarkAsync(Guid.NewGuid().ToString());

            _landmarkRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Landmark>()), Times.Never);
        }

        [Test]
        public async Task GetLandmarkForEditReturnsNullWithNotExistingId() 
        {
            List<Landmark> landmarks = new List<Landmark>();
            IQueryable<Landmark> queryList = landmarks.BuildMock();

            _landmarkRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            LandmarkEditViewModel? model = await _landmarkService.GetLandmarkForEditAsync(Guid.NewGuid().ToString());

            Assert.IsNull(model);
        }

        [Test]
        public async Task GetLandmarkForEditReturnsExistingLandmark() 
        {
            Landmark newLandmark = new Landmark() 
            {
                Id = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1"),
                Name = "Name",
                Description = "Description",
                ImageUrl = null,
                LocationName = "LocationName",
                DestinationId = Guid.Parse("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd")
            };

            List<Landmark> landmarks = new List<Landmark>() { newLandmark };
            IQueryable<Landmark> queryList = landmarks.BuildMock();

            _landmarkRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            LandmarkEditViewModel? model = await _landmarkService.GetLandmarkForEditAsync("271cf215-ce36-4fc9-87e5-c71e214af3a1");

            Assert.IsNotNull(model);
            Assert.That(newLandmark.Name, Is.EqualTo(model.Name));
            Assert.That(newLandmark.Description, Is.EqualTo(model.Description));
            Assert.That(newLandmark.LocationName, Is.EqualTo(model.Location));
        }

        [Test]
        public async Task GetLandmarksForTourReturnsNullIfIdIsInvalid() 
        {
            List<Landmark> landmarks = new List<Landmark>();
            IQueryable<Landmark> queryList = landmarks.BuildMock();

            _landmarkRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            IEnumerable<GetLandmarksForToursViewModel>? model = await _landmarkService.GetLandmarksForTourAsync(Guid.NewGuid().ToString());

            Assert.IsEmpty(model);
        }

        [Test]
        public async Task GetLandmarkForTourReturnsLandmarkWithCorrectId() 
        {
            Landmark newLandmark = new Landmark()
            {
                Id = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1"),
                Name = "Name",
                Description = "Description",
                ImageUrl = null,
                LocationName = "LocationName",
                DestinationId = Guid.Parse("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd")
            };

            List<Landmark> landmarks = new List<Landmark>() { newLandmark };
            IQueryable<Landmark> queryList = landmarks.BuildMock();

            _landmarkRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            IEnumerable<GetLandmarksForToursViewModel>? model = await _landmarkService.GetLandmarksForTourAsync("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd");

            Assert.IsNotNull(model);
            Assert.That(model.Count(), Is.EqualTo(landmarks.Count()));

            foreach (var land in model)
            {
                GetLandmarksForToursViewModel? landVm = model.FirstOrDefault(l => l.Id.ToString() == land.Id.ToString());

                Assert.IsNotNull(landVm);
                Assert.That(land.Name, Is.EqualTo(landVm.Name));
            }
        }

        [Test]
        public async Task GetLandmarkDetailsReturnNullWithWrongId() 
        {
            Landmark newLandmark = new Landmark()
            {
                Id = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1"),
                Name = "Name",
                Description = "Description",
                ImageUrl = null,
                LocationName = "LocationName",
                DestinationId = Guid.Parse("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd")
            };

            List<Landmark> landmarks = new List<Landmark>() { newLandmark };
            IQueryable<Landmark> queryList = landmarks.BuildMock();

            _landmarkRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            LandmarkDetailsViewModel? land = await _landmarkService.GetLandmarkDetailAsync("eeb68d1e-2d9e-46e5-a827-3f78fb93a07b", "a6959478-fc72-432f-9b55-a0b6d33e457e");

            Assert.IsNull(land);
        }

        [Test]
        public async Task GetLandmarDetailsReturnNullWithNullId() 
        {
            Landmark newLandmark = new Landmark()
            {
                Id = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1"),
                Name = "Name",
                Description = "Description",
                ImageUrl = null,
                LocationName = "LocationName",
                DestinationId = Guid.Parse("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd")
            };

            List<Landmark> landmarks = new List<Landmark>() { newLandmark };
            IQueryable<Landmark> queryList = landmarks.BuildMock();

            _landmarkRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            LandmarkDetailsViewModel? land = await _landmarkService.GetLandmarkDetailAsync(null, null);

            Assert.IsNull(land);
        }

        [Test]
        public async Task GetLandmarkReturnsLandmarkWithTheCorrectId() 
        {
            var landmarkId = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1");
            var destinationId = Guid.Parse("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd");

            var destination = new Destination
            {
                Id = destinationId,
                CountryName = "BG"
            };

            var landmark = new Landmark
            {
                Id = landmarkId,
                Name = "Name",
                Description = "Description",
                ImageUrl = null,
                LocationName = "Loc",
                Destination = destination,
                DestinationId = destinationId,
                UserLandmarks = new List<UserLandmark>()
            };

            var landmarkList = new List<Landmark> { landmark }.BuildMock();

            _landmarkRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(landmarkList);

            LandmarkDetailsViewModel? land = await _landmarkService.GetLandmarkDetailAsync(null, landmarkId.ToString());

            Assert.IsNotNull(land);
            Assert.That(land.Id, Is.EqualTo(landmarkId.ToString()));
            Assert.That(land.Title, Is.EqualTo(landmark.Name));
            Assert.That(land.Description, Is.EqualTo(landmark.Description));
        }

        [Test]
        public async Task GetAllLandmarksReturnEmptyListWhenCollectionIsEmpty() 
        {
            List<Landmark> landmarks = new List<Landmark>();
            IQueryable<Landmark> queryList = landmarks.BuildMock();

            _landmarkRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            IEnumerable<GetAllLandmarksViewModel> allLand = await _landmarkService.GetAllLandmarksAsync(null);

            Assert.IsEmpty(allLand);
        }

        [Test]
        public async Task GetAllLandmarksReturnList() 
        {
            var landmarkId = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1");
            var destinationId = Guid.Parse("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd");

            var destination = new Destination
            {
                Id = destinationId,
                CountryName = "BG"
            };

            var landmark = new Landmark
            {
                Id = landmarkId,
                Name = "Name",
                Description = "Description",
                ImageUrl = null,
                LocationName = "Loc",
                Destination = destination,
                DestinationId = destinationId,
                UserLandmarks = new List<UserLandmark>()
            };

            var landmarkList = new List<Landmark> { landmark }.BuildMock();

            _landmarkRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(landmarkList);

            IEnumerable<GetAllLandmarksViewModel> allLand = await _landmarkService.GetAllLandmarksAsync(null);

            Assert.IsNotEmpty(allLand);
            Assert.That(landmarkList.Count(), Is.EqualTo(allLand.Count()));
            foreach (var land in allLand)
            {
                GetAllLandmarksViewModel landVm = allLand.FirstOrDefault(l => l.Id.ToString() == land.Id.ToString() );

                Assert.IsNotNull(landVm);
                Assert.That(land.Name, Is.EqualTo(landVm.Name));
            }
        }

        [Test]
        public async Task GetAllLandmarksByDestIdReturnEmptyListWhenThereIsNoMatch() 
        {
            List<Landmark> landmarks = new List<Landmark>();
            IQueryable<Landmark> queryList = landmarks.BuildMock();

            _landmarkRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            IEnumerable<GetAllLandmarksViewModel> allLand = await _landmarkService.GetAllLandmarksByDestinationIdAsync(null, "ca67ea43-c1b6-4a0e-b501-bd5393bb98fd");

            Assert.IsEmpty(allLand);
        }

        [Test]
        public async Task GetAllLandmarksByDestIdReturnLandmarksWhenIdIsCorrect() 
        {
            var landmarkId = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1");
            var destinationId = Guid.Parse("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd");

            var destination = new Destination
            {
                Id = destinationId,
                CountryName = "BG"
            };

            var landmark = new Landmark
            {
                Id = landmarkId,
                Name = "Name",
                Description = "Description",
                ImageUrl = null,
                LocationName = "Loc",
                Destination = destination,
                DestinationId = destinationId,
                UserLandmarks = new List<UserLandmark>()
            };

            var landmarkList = new List<Landmark> { landmark }.BuildMock();

            _landmarkRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(landmarkList);

            IEnumerable<GetAllLandmarksViewModel> allLand = await _landmarkService.GetAllLandmarksByDestinationIdAsync(null, destinationId.ToString());

            Assert.IsNotEmpty(allLand);
            Assert.That(landmarkList.Count(), Is.EqualTo(allLand.Count()));
            foreach (var land in allLand)
            {
                GetAllLandmarksViewModel landVm = allLand.FirstOrDefault(l => l.Id.ToString() == land.Id.ToString());

                Assert.IsNotNull(landVm);
                Assert.That(land.Name, Is.EqualTo(landVm.Name));
            }
        }

        [Test]
        public async Task GetAllLandmarksForAdminReturnEmptyListWhenTherAreNoLandmarks() 
        {
            List<Landmark> landmarks = new List<Landmark>();
            IQueryable<Landmark> queryList = landmarks.BuildMock();

            _landmarkRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(queryList);

            IEnumerable<GetAllLandmarksViewModel> allLand = await _landmarkService.GetAllLandmarksForAdmin(null);

            Assert.IsEmpty(allLand);
        }

        [Test]
        public async Task GetAllLandmarksForAdminReturnsAllLandmarks() 
        {
            var landmarkId = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1");
            var landmarkId1 = Guid.Parse("a4c32ad7-eaa4-4498-9986-24518b4d022a");
            var destinationId = Guid.Parse("ca67ea43-c1b6-4a0e-b501-bd5393bb98fd");

            var destination = new Destination
            {
                Id = destinationId,
                CountryName = "BG"
            };

            var landmark = new Landmark
            {
                Id = landmarkId,
                Name = "Name",
                Description = "Description",
                ImageUrl = null,
                LocationName = "Loc",
                Destination = destination,
                DestinationId = destinationId,
                UserLandmarks = new List<UserLandmark>()
            };
            var landmark1 = new Landmark
            {
                Id = landmarkId1,
                Name = "Name",
                Description = "Description",
                ImageUrl = null,
                LocationName = "Loc",
                Destination = destination,
                DestinationId = destinationId,
                UserLandmarks = new List<UserLandmark>(),
                IsDeleted = true
            };


            var landmarkList = new List<Landmark> { landmark, landmark1 }.BuildMock();

            _landmarkRepositoryMock
                .Setup(l => l.GetAllAttached())
                .Returns(landmarkList);

            IEnumerable<GetAllLandmarksViewModel> allLand = await _landmarkService.GetAllLandmarksForAdmin(null);

            Assert.IsNotEmpty(allLand);
            Assert.That(landmarkList.Count(), Is.EqualTo(allLand.Count()));
            foreach (var land in allLand)
            {
                GetAllLandmarksViewModel landVm = allLand.FirstOrDefault(l => l.Id.ToString() == land.Id.ToString());

                Assert.IsNotNull(landVm);
                Assert.That(land.Name, Is.EqualTo(landVm.Name));
            }
        }

        [Test]
        public async Task SaveEditChangesAsyncNullModelReturnsFalse()
        {
            var result = await _landmarkService.SaveEditChangesAsync(null);

            Assert.IsFalse(result);
            _landmarkRepositoryMock.Verify(r => r.GetAllAttached(), Times.Never);
            _landmarkRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Landmark>()), Times.Never);
        }

        [Test]
        public async Task SaveEditChangesAsyncValidModelUpdatesLandmarkAndReturnsTrue()
        {
            var model = new LandmarkEditViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Updated Name",
                Description = "Updated Description",
                ImageUrl = "image.jpg",
                Location = "New Location",
                DestinationId = Guid.NewGuid().ToString()
            };

            var landmark = new Landmark { Id = Guid.Parse(model.Id) };
            var destination = new Destination { Id = Guid.Parse(model.DestinationId) };

            var mockList = new List<Landmark> { landmark }.BuildMock();

            _landmarkRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockList);

            _destinationRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Destination, bool>>>()))
                .ReturnsAsync(destination);

            _landmarkRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Landmark>()))
                .ReturnsAsync(true);

            var result = await _landmarkService.SaveEditChangesAsync(model);

            Assert.IsTrue(result);
            Assert.AreEqual(model.Name, landmark.Name);
            Assert.AreEqual(model.Description, landmark.Description);
            Assert.AreEqual(model.ImageUrl, landmark.ImageUrl);
            Assert.AreEqual(model.Location, landmark.LocationName);
            Assert.AreEqual(Guid.Parse(model.DestinationId), landmark.DestinationId);
        }

        [Test]
        public async Task SaveEditChangesAsyncLandmarkNotFoundReturnsFalse()
        {
            var model = new LandmarkEditViewModel
            {
                Id = Guid.NewGuid().ToString(),
                DestinationId = Guid.NewGuid().ToString()
            };

            var mockList = new List<Landmark>().BuildMock();

            _landmarkRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockList);

            _destinationRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Destination, bool>>>()))
                .ReturnsAsync((Destination?)null);

            var result = await _landmarkService.SaveEditChangesAsync(model);

            Assert.IsFalse(result);
            _landmarkRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Landmark>()), Times.Never);
        }

        [Test]
        public async Task SaveEditChangesAsyncDestinationNotFoundReturnsFalse()
        {
            var model = new LandmarkEditViewModel
            {
                Id = Guid.NewGuid().ToString(),
                DestinationId = Guid.NewGuid().ToString()
            };

            var landmark = new Landmark { Id = Guid.Parse(model.Id) };
            var mockList = new List<Landmark> { landmark }.BuildMock();

            _landmarkRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockList);

            _destinationRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Destination, bool>>>()))
                .ReturnsAsync((Destination?)null);

            var result = await _landmarkService.SaveEditChangesAsync(model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SaveEditChangesAsyncUpdateFailsReturnsFalse()
        {
            var model = new LandmarkEditViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Name",
                Description = "Desc",
                ImageUrl = "url",
                Location = "loc",
                DestinationId = Guid.NewGuid().ToString()
            };

            var landmark = new Landmark { Id = Guid.Parse(model.Id) };
            var destination = new Destination { Id = Guid.Parse(model.DestinationId) };

            var mockList = new List<Landmark> { landmark }.BuildMock();

            _landmarkRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockList);

            _destinationRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Destination, bool>>>()))
                .ReturnsAsync(destination);

            _landmarkRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Landmark>()))
                .ReturnsAsync(false);

            var result = await _landmarkService.SaveEditChangesAsync(model);

            Assert.IsFalse(result);
        }
    }
}
