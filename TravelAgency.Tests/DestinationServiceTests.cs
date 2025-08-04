using Moq;
using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;
using TravelAgency.Service.Core;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.DestinationModels;
using MockQueryable;
using MockQueryable.Moq;


namespace TravelAgency.Tests
{
    [TestFixture]
    public class DestinationServiceTests
    {
        private Mock<IDestinationRepository> _destinationRepositoryMock;
        private IDestinationService _destinationService;

        [SetUp]
        public void Setup()
        {
            _destinationRepositoryMock = new Mock<IDestinationRepository>(MockBehavior.Strict);
            _destinationService = new DestinationService(_destinationRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            Assert.Pass();
        }

        [Test]
        public async Task AddDestinationReturnsFalseWithNullModelPassed()
        {
            bool result = await _destinationService.AddDestinationAsync(null);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddDestinationReturnsTrueIfModelIsAdded()
        {
            var model = new AddDestinationViewModel
            {
                Name = "France",
                Description = "Romantic destination",
                ImageUrl = null
            };

            _destinationRepositoryMock
                .Setup(repo => repo.AddAsync(It.Is<Destination>(d =>
                    d.CountryName == model.Name &&
                    d.Description == model.Description &&
                    d.ImageUrl == model.ImageUrl)))
                .Returns(Task.CompletedTask);

            var result = await _destinationService.AddDestinationAsync(model);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAllDestinationShoudReturnEmptyCollection()
        {
            List<Destination> emptyList = new List<Destination>();
            IQueryable<Destination> emptyQueryable = emptyList.BuildMock();

            _destinationRepositoryMock
                .Setup(d => d.GetAllAttached())
                .Returns(emptyQueryable);

            IEnumerable<AllDestinationsViewModel> emptyViewModel = await _destinationService.GetAllDestinationsAsync(null);

            Assert.IsNotNull(emptyViewModel);
            Assert.That(emptyViewModel.Count(), Is.EqualTo(emptyList.Count()));
        }

        [Test]
        public async Task GetAllDestinationReturnsCollectionWhenNotEmpty()
        {
            List<Destination> destList = new List<Destination>()
            {
                new Destination
                {
                    Id = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1"),
                    CountryName = "Name",
                    ImageUrl = null
                }
            };
            IQueryable<Destination> list = destList.BuildMock();

            _destinationRepositoryMock
                .Setup(d => d.GetAllAttached())
                .Returns(list);

            IEnumerable<AllDestinationsViewModel> destViewModel = await _destinationService.GetAllDestinationsAsync(null);

            Assert.IsNotNull(destViewModel);
            Assert.That(destViewModel.Count(), Is.EqualTo(destList.Count()));

            foreach (var dest in destList)
            {
                AllDestinationsViewModel? destVm = destViewModel.FirstOrDefault(d => d.Id.ToString() == dest.Id.ToString());

                Assert.IsNotNull(destVm);
                Assert.That(dest.CountryName, Is.EqualTo(destVm.Name));
            }
        }

        [Test]
        public async Task GetAllDestinationReturnsDestinationFilteredBySearchWord()
        {
            List<Destination> destList = new List<Destination>()
            {
                new Destination
                {
                    Id = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1"),
                    CountryName = "Bulgaria",
                    ImageUrl = null
                }
            };
            IQueryable<Destination> list = destList.BuildMock();

            _destinationRepositoryMock
                .Setup(d => d.GetAllAttached())
                .Returns(list);

            IEnumerable<AllDestinationsViewModel> destViewModel = await _destinationService.GetAllDestinationsAsync("Bul");

            Assert.IsNotNull(destViewModel);
            Assert.That(destViewModel.Count(), Is.EqualTo(destList.Count()));

            foreach (var dest in destList)
            {
                AllDestinationsViewModel? destVm = destViewModel.FirstOrDefault(d => d.Id.ToString() == dest.Id.ToString());

                Assert.IsNotNull(destVm);
                Assert.That(dest.CountryName, Is.EqualTo(destVm.Name));
            }
        }

        [Test]
        public async Task GetAllDestinationReturnsEmptyCollectionWhenWordForFilterDontMatch()
        {
            List<Destination> destList = new List<Destination>()
            {
                new Destination
                {
                    Id = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1"),
                    CountryName = "Bulgaria",
                    ImageUrl = null
                }
            };
            IQueryable<Destination> list = destList.BuildMock();

            _destinationRepositoryMock
                .Setup(d => d.GetAllAttached())
                .Returns(list);

            IEnumerable<AllDestinationsViewModel> destViewModel = await _destinationService.GetAllDestinationsAsync("A");

            Assert.IsEmpty(destViewModel);
        }

        [Test]
        public async Task DeleteOrRestoreDestinationAsyncWithValidIdAndMatchingDestinationShouldToggleIsDeletedAndUpdate()
        {
            var id = "271cf215-ce36-4fc9-87e5-c71e214af3a1";
            var destination = new Destination
            {
                Id = Guid.Parse(id),
                IsDeleted = false
            };

            List<Destination> destList = new List<Destination>()
            {
                destination
            };

            IQueryable<Destination> destinationList = destList.BuildMock();

            _destinationRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(destinationList);

            _destinationRepositoryMock
                .Setup(r => r.UpdateAsync(It.Is<Destination>(d => d.Id == destination.Id && d.IsDeleted == true)))
                .ReturnsAsync(true);

            await _destinationService.DeleteOrRestoreDestinationAsync(id);

            Assert.IsTrue(destination.IsDeleted);
            _destinationRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Destination>()), Times.Once);
        }

        [Test]
        public async Task DeleteOrRestoreDestinationAsyncWithNullIdShouldDoNothing()
        {
            await _destinationService.DeleteOrRestoreDestinationAsync(null);

            _destinationRepositoryMock.Verify(r => r.GetAllAttached(), Times.Never);
            _destinationRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Destination>()), Times.Never);
        }

        [Test]
        public async Task DeleteOrRestoreDestinationAsyncWithNoMatchingDestinationShouldDoNothing()
        {
            var id = Guid.NewGuid().ToString();

            var emptyList = new List<Destination>();
            IQueryable<Destination> queryList = emptyList.BuildMock();

            _destinationRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(queryList);

            await _destinationService.DeleteOrRestoreDestinationAsync(id);

            _destinationRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Destination>()), Times.Never);
        }

        [Test]
        public async Task GetAllDestinationForAdminShoudReturnEmptyCollection()
        {
            List<Destination> emptyList = new List<Destination>();
            IQueryable<Destination> emptyQueryable = emptyList.BuildMock();

            _destinationRepositoryMock
                .Setup(d => d.GetAllAttached())
                .Returns(emptyQueryable);

            IEnumerable<AllDestinationsViewModel> emptyViewModel = await _destinationService.GetAllDestinationsForAdminAsync(null);

            Assert.IsNotNull(emptyViewModel);
            Assert.That(emptyViewModel.Count(), Is.EqualTo(emptyList.Count()));
        }

        [Test]
        public async Task GetAllDestinationForAdminReturnsCollectionWhenNotEmpty()
        {
            List<Destination> destList = new List<Destination>()
            {
                new Destination
                {
                    Id = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1"),
                    CountryName = "Name",
                    ImageUrl = null,
                    IsDeleted = true
                }
            };
            IQueryable<Destination> list = destList.BuildMock();

            _destinationRepositoryMock
                .Setup(d => d.GetAllAttached())
                .Returns(list);

            IEnumerable<AllDestinationsViewModel> destViewModel = await _destinationService.GetAllDestinationsForAdminAsync(null);

            Assert.IsNotNull(destViewModel);
            Assert.That(destViewModel.Count(), Is.EqualTo(destList.Count()));

            foreach (var dest in destList)
            {
                AllDestinationsViewModel? destVm = destViewModel.FirstOrDefault(d => d.Id.ToString() == dest.Id.ToString());

                Assert.IsNotNull(destVm);
                Assert.That(dest.CountryName, Is.EqualTo(destVm.Name));
            }
        }

        [Test]
        public async Task GetDestinationDetailsReturnsNullWithNotExistingId() 
        {
            List<Destination> emptyList = new List<Destination>();
            IQueryable<Destination> emptyQueryable = emptyList.BuildMock();

            _destinationRepositoryMock
                .Setup(d => d.GetAllAttached())
                .Returns(emptyQueryable);

            DestinationDetailViewModel? model = await _destinationService.GetDestinationDetailsAsync("271cf215-ce36-4fc9-87e5-c71e214af3a1");

            Assert.IsNull(model);
        }

        [Test]
        public async Task GetDestinationDetailsReturnRightDestination() 
        {
            var dest = new Destination
            {
                Id = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1"),
                CountryName = "Name",
                Description = "Description",
            };

            List<Destination> destList = new List<Destination>() 
            {
                dest
            };
            IQueryable<Destination> destQuery = destList.BuildMock();

            _destinationRepositoryMock
                .Setup(d => d.GetAllAttached())
                .Returns(destQuery);

            DestinationDetailViewModel? model = await _destinationService.GetDestinationDetailsAsync("271cf215-ce36-4fc9-87e5-c71e214af3a1");

            Assert.IsNotNull(model);
            Assert.That(dest.CountryName, Is.EqualTo(model.Title));
            Assert.That(dest.Description, Is.EqualTo(model.Description));
        }

        [Test]
        public async Task GetDestinationForEditReturnsNullWithNotExistingId() 
        {
            List<Destination> emptyList = new List<Destination>();
            IQueryable<Destination> emptyQueryable = emptyList.BuildMock();

            _destinationRepositoryMock
                .Setup(d => d.GetAllAttached())
                .Returns(emptyQueryable);

            DestinationEditViewModel? model = await _destinationService.GetDestinationForEditAsync("271cf215-ce36-4fc9-87e5-c71e214af3a1");

            Assert.IsNull(model);
        }

        [Test]
        public async Task GetDestinationForEditReturnsRightDestination() 
        {
            var dest = new Destination
            {
                Id = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1"),
                CountryName = "Name",
                Description = "Description",
            };

            List<Destination> destList = new List<Destination>()
            {
                dest
            };
            IQueryable<Destination> destQuery = destList.BuildMock();

            _destinationRepositoryMock
                .Setup(d => d.GetAllAttached())
                .Returns(destQuery);

            DestinationEditViewModel? model = await _destinationService.GetDestinationForEditAsync("271cf215-ce36-4fc9-87e5-c71e214af3a1");

            Assert.IsNotNull(model);
            Assert.That(dest.CountryName, Is.EqualTo(model.Name));
            Assert.That(dest.Description, Is.EqualTo(model.Description));
        }

        [Test]
        public async Task SaveChangesReturnFalseWhenNullIsPassed() 
        {
            var result = await _destinationService.SaveEditChangesAsync(null);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SaveChangesReturnsFalseWithInvalidId() 
        {
            DestinationEditViewModel dest = new DestinationEditViewModel
            {
                Id = "271cf215-ce36-4fc9-87e5-c71e214af3a1",
                Name = "Name",
                Description = "Description",
            };

            List<Destination> emptyList = new List<Destination>();
            IQueryable<Destination> emptyQueryable = emptyList.BuildMock();

            _destinationRepositoryMock
                .Setup(d => d.GetAllAttached())
                .Returns(emptyQueryable);

            var result = await _destinationService.SaveEditChangesAsync(dest);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SaveChangesReturnsTrueWithValidModel() 
        {
            DestinationEditViewModel destModel = new DestinationEditViewModel
            {
                Id = "271cf215-ce36-4fc9-87e5-c71e214af3a1",
                Name = "Name",
                Description = "Description",
            };

            var dest = new Destination
            {
                Id = Guid.Parse("271cf215-ce36-4fc9-87e5-c71e214af3a1"),
                CountryName = "Name",
                Description = "Description",
            };

            List<Destination> destList = new List<Destination>()
            {
                dest
            };
            IQueryable<Destination> destQuery = destList.BuildMock();

            _destinationRepositoryMock
                .Setup(d => d.GetAllAttached())
                .Returns(destQuery);

            _destinationRepositoryMock
           .Setup(r => r.UpdateAsync(It.Is<Destination>(d =>
               d.Id.ToString() == destModel.Id &&
               d.CountryName == destModel.Name &&
               d.Description == destModel.Description &&
               d.ImageUrl == destModel.ImageUrl)))
           .ReturnsAsync(true);

            var result = await _destinationService.SaveEditChangesAsync(destModel);

            Assert.IsTrue(result);
        }

    }
}
