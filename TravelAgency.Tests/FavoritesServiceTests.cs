using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;
using TravelAgency.Service.Core;

namespace TravelAgency.Tests
{
    [TestFixture]
    public class FavoritesServiceTests
    {
        private Mock<IUserLandmarkRepository> _userLandmarkRepositoryMock;
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private FavoritesService _favoritesService;

        [SetUp]
        public void Setup()
        {
            _userLandmarkRepositoryMock = new Mock<IUserLandmarkRepository>(MockBehavior.Strict);

            var store = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            _favoritesService = new FavoritesService(_userLandmarkRepositoryMock.Object, _userManagerMock.Object);
        }

        [Test]
        public async Task AddToFavoritesAsyncUserAndLandmarkValidNotAlreadyFavoriteAddsToRepository()
        {
            var userId = "user123";
            var landmarkId = Guid.NewGuid().ToString();

            _userLandmarkRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<UserLandmark, bool>>>()))
                .ReturnsAsync((UserLandmark?)null);

            _userLandmarkRepositoryMock
                .Setup(r => r.AddAsync(It.Is<UserLandmark>(ul => ul.UserId == userId && ul.LandmarkId.ToString() == landmarkId)))
                .Returns(Task.CompletedTask);

            await _favoritesService.AddToFavoritesAsync(userId, landmarkId);

            _userLandmarkRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserLandmark>()), Times.Once);
        }

        [Test]
        public async Task AddToFavoritesAsyncDoesNotAddAgainIfAlreadyAdded()
        {
            var userId = "user123";
            var landmarkId = Guid.NewGuid().ToString();

            var existing = new UserLandmark { UserId = userId, LandmarkId = Guid.Parse(landmarkId) };

            _userLandmarkRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<UserLandmark, bool>>>()))
                .ReturnsAsync(existing);

            await _favoritesService.AddToFavoritesAsync(userId, landmarkId);

            _userLandmarkRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserLandmark>()), Times.Never);
        }

        [Test]
        public async Task AddToFavoritesAsyncNullUserOrLandmarkDoesNothing()
        {
            await _favoritesService.AddToFavoritesAsync(null, null);
            await _favoritesService.AddToFavoritesAsync("user123", null);
            await _favoritesService.AddToFavoritesAsync(null, Guid.NewGuid().ToString());

            _userLandmarkRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserLandmark>()), Times.Never);
            _userLandmarkRepositoryMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<UserLandmark, bool>>>()), Times.Never);
        }

        [Test]
        public async Task GetAllFavoritesLandmarksAsyncUserExistsReturnsFavorites()
        {
            string userId = "user123";
            var user = new IdentityUser { Id = userId };

            var landmarks = new List<UserLandmark>
            {
                   new UserLandmark
                    {
                        UserId = userId,
                        LandmarkId = Guid.NewGuid(),
                        Landmark = new Landmark
                            {
                                Name = "Landmark A",
                                LocationName = "Location A",
                                ImageUrl = "image.jpg",
                                Destination = new Destination { CountryName = "Country A" }
                            }
                    }
             };

            var landmarkQueryable = landmarks.AsQueryable().BuildMock();

            _userManagerMock
                .Setup(m => m.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _userLandmarkRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(landmarkQueryable);

            var result = await _favoritesService.GetAllFavoritesLandmarksAsync(userId);

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Landmark A"));
        }

        [Test]
        public async Task GetAllFavoritesLandmarksAsyncUserNotFoundReturnsNull()
        {
            string userId = "nonexistent";

            _userManagerMock
                .Setup(m => m.FindByIdAsync(userId))
                .ReturnsAsync((IdentityUser?)null);

            var result = await _favoritesService.GetAllFavoritesLandmarksAsync(userId);

            Assert.IsNull(result);
            _userLandmarkRepositoryMock.Verify(r => r.GetAllAttached(), Times.Never);
        }

        [Test]
        public async Task GetAllFavoritesLandmarksAsyncNullUserIdReturnsNull()
        {
            var result = await _favoritesService.GetAllFavoritesLandmarksAsync(null);

            Assert.IsNull(result);
            _userManagerMock.Verify(m => m.FindByIdAsync(It.IsAny<string>()), Times.Once);
            _userLandmarkRepositoryMock.Verify(r => r.GetAllAttached(), Times.Never);
        }

        [Test]
        public async Task RemoveFromFavoritesAsyncWhenFavoriteExistsShouldDeleteIt()
        {
            string userId = "user123";
            string landmarkId = Guid.NewGuid().ToString();

            var favorite = new UserLandmark
            {
                UserId = userId,
                LandmarkId = Guid.Parse(landmarkId)
            };

            _userLandmarkRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<UserLandmark, bool>>>()))
                .ReturnsAsync(favorite);

            _userLandmarkRepositoryMock
                .Setup(r => r.HardDeleteAsync(favorite))
                .ReturnsAsync(true);

            await _favoritesService.RemoveFromFavoritesAsync(userId, landmarkId);

            _userLandmarkRepositoryMock.Verify(r => r.HardDeleteAsync(favorite), Times.Once);
        }

        [Test]
        public async Task RemoveFromFavoritesAsyncWhenFavoriteNotFoundShouldNotDelete()
        {
            string userId = "user123";
            string landmarkId = Guid.NewGuid().ToString();

            _userLandmarkRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<UserLandmark, bool>>>()))
                .ReturnsAsync((UserLandmark?)null);

            await _favoritesService.RemoveFromFavoritesAsync(userId, landmarkId);

            _userLandmarkRepositoryMock.Verify(r => r.HardDeleteAsync(It.IsAny<UserLandmark>()), Times.Never);
        }

        [Test]
        public async Task RemoveFromFavoritesAsyncWithNullInputShouldNotThrow()
        {
            await _favoritesService.RemoveFromFavoritesAsync(null, null);

            _userLandmarkRepositoryMock.Verify(
                r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<UserLandmark, bool>>>()), Times.Never);

            _userLandmarkRepositoryMock.Verify(
                r => r.HardDeleteAsync(It.IsAny<UserLandmark>()), Times.Never);
        }
    }
}
