using Moq;
using VAArtGalleryWebAPI.Application.Commands;
using VAArtGalleryWebAPI.Application.Queries;
using VAArtGalleryWebAPI.Domain.Entities;
using VAArtGalleryWebAPI.Domain.Interfaces;
using VAArtGalleryWebAPI.WebApi.Models;

namespace VAArGalleryWebAPITest
{
    public class Tests
    {
        private ArtGallery g1 = new ArtGallery("Gallery One", "Beja", "Baltazar Braz");
        private ArtGallery g2 = new ArtGallery("Gallery Two", "Bragança", "Bernardo Beltrão");
        private ArtWork a1 = new ArtWork("obra 1", "artista 1", 1900, 1000);
        private ArtWork a2 = new ArtWork("obra 2", "artista 1", 1910, 1500);
        private ArtWork a3 = new ArtWork("obra 3", "artista 2", 1920, 2000);
        private ArtWork a4 = new ArtWork("obra 4", "artista 3", 1930, 5000);
        private ArtWork a5 = new ArtWork("obra 5", "artista 4", 1940, 10000);

        [SetUp]
        public void Setup()
        {
            SetupGalleriesAndWorks();
        }

        [Test]
        public async Task Test_Returns_the_galleries_successfully()
        {
            var r = await new GetAllArtGalleriesQueryHandler(NormalArtGalleryRepositoryMock().Object).Handle(new GetAllArtGalleriesQuery(), CancellationToken.None);

            Assert.That(r, Is.Not.Null);
            Assert.That(r.Count, Is.EqualTo(2));
            Assert.That(r.First().Manager, Is.EqualTo("Baltazar Braz"));
        }

        [Test]
        public async Task Test_Returns_null_when_getting_works_from_inexisting_gallery()
        {
            var r = await new GetArtGalleryArtWorksQueryHandler(InvalidGalleryArtWorksRepositoryMock().Object).Handle(new GetArtGalleryArtWorksQuery(Guid.NewGuid()), CancellationToken.None);

            Assert.That(r, Is.Null);
        }

        [Test]
        public async Task Test_Returns_all_art_works_from_a_valid_gallery()
        {
            var r = await new GetArtGalleryArtWorksQueryHandler(NormalArtWorksRepositoryMock().Object).Handle(new GetArtGalleryArtWorksQuery(Guid.NewGuid()), CancellationToken.None);

            Assert.That(r, Is.Not.Null);
            Assert.That(r.Count(), Is.EqualTo(2));
            Assert.That(r.First(), Is.EqualTo(a1));
        }

        #region GetArtGalleryByIdQuery

        [Test]
        public async Task Test_GetArtGalleryById_Handle_ShouldReturnArtGallery_WhenArtGalleryExists()
        {
            // Arrange
            var query = new GetArtGalleryByIdQuery(g1.Id);

            // Act
            var result = await new GetArtGalleryByIdQueryHandler(NormalArtGalleryRepositoryMock().Object).Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(g1.Id));
            Assert.That(result.Name, Is.EqualTo(g1.Name));
            Assert.That(result.City, Is.EqualTo(g1.City));
            Assert.That(result.Manager, Is.EqualTo(g1.Manager));
            Assert.That(result.NbrOfArtWorksOnDisplay, Is.EqualTo(g1.ArtWorksOnDisplay?.Count ?? 0));
        }

        [Test]
        public async Task Test_GetArtGalleryById_Handle_ShouldReturnNull_WhenArtGalleryDoesNotExist()
        {
            // Arrange
            var artGalleryId = Guid.NewGuid();
            var query = new GetArtGalleryByIdQuery(artGalleryId);

            // Act
            var result = await new GetArtGalleryByIdQueryHandler(NormalArtGalleryRepositoryMock().Object).Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Test_GetArtGalleryByIdQuery_Handle_ShouldReturnNull_WhenArgumentNullExceptionThrown()
        {
            // Arrange
            var invalidArtGalleryRepositoryMock = InvalidArtGalleryRepositoryMock();
            var handler = new GetArtGalleryByIdQueryHandler(invalidArtGalleryRepositoryMock.Object);
            var query = new GetArtGalleryByIdQuery(g1.Id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Test_GetArtGalleryById_Handle_ShouldThrowException_ForUnknownExceptions()
        {
            // Arrange
            var artGalleryId = Guid.NewGuid();
            var unknownExceptionMock = new Mock<IArtGalleryRepository>();
            unknownExceptionMock.Setup(repo => repo.GetArtGalleryByIdAsync(artGalleryId, It.IsAny<CancellationToken>()))
                                .ThrowsAsync(new Exception("Unknown error"));
            var handler = new GetArtGalleryByIdQueryHandler(unknownExceptionMock.Object);
            var query = new GetArtGalleryByIdQuery(artGalleryId);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
        }

        #endregion GetArtGalleryByIdQuery

        #region GetArtWorkById

        [Test]
        public async Task Test_GetArtWorkById_Handle_ShouldReturnArtWork_WhenArtWorkExists()
        {
            // Arrange
            var query = new GetArtWorkByIdQuery(a1.Id);

            // Act
            var result = await new GetArtWorkByIdQueryHandler(NormalArtWorksRepositoryMock().Object).Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(a1.Id));
            Assert.That(result.Name, Is.EqualTo(a1.Name));
            Assert.That(result.Author, Is.EqualTo(a1.Author));
            Assert.That(result.CreationYear, Is.EqualTo(a1.CreationYear));
            Assert.That(result.AskPrice, Is.EqualTo(a1.AskPrice));
        }

        [Test]
        public async Task Test_GetArtWorkById_Handle_ShouldReturnNull_WhenArtWorkDoesNotExist()
        {
            // Arrange
            var artWorkId = Guid.NewGuid();
            var query = new GetArtWorkByIdQuery(artWorkId);

            // Act
            var result = await new GetArtWorkByIdQueryHandler(NormalArtWorksRepositoryMock().Object).Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Test_GetArtWorkById_Handle_ShouldReturnNull_WhenArgumentNullExceptionThrown()
        {
            // Arrange
            var invalidArtWorkRepositoryMock = InvalidGalleryArtWorksRepositoryMock();
            var handler = new GetArtWorkByIdQueryHandler(invalidArtWorkRepositoryMock.Object);
            var query = new GetArtWorkByIdQuery(a1.Id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Test_GetArtWorkById_Handle_ShouldThrowException_ForUnknownExceptions()
        {
            // Arrange
            var artWorkId = Guid.NewGuid();
            var unknownExceptionMock = new Mock<IArtWorkRepository>();
            unknownExceptionMock.Setup(repo => repo.GetArtWorkyByIdAsync(artWorkId, It.IsAny<CancellationToken>()))
                                .ThrowsAsync(new Exception("Unknown error"));
            var handler = new GetArtWorkByIdQueryHandler(unknownExceptionMock.Object);
            var query = new GetArtWorkByIdQuery(artWorkId);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
        }

        #endregion GetArtWorkById

        #region CreateArtGalleryCommand

        [Test]
        public async Task Test_CreateArtGalleryCommand_Handle_ShouldReturnCreateArtGalleryResult_WhenArtGalleryIsCreated()
        {
            // Arrange
            var request = new CreateArtGalleryRequest("Test Gallery", "Test City", "Test Manager");
            var command = new CreateArtGalleryCommand(request);
            var handler = new CreateArtGalleryCommandHandler(NormalArtGalleryRepositoryMock().Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.Name, Is.EqualTo(request.Name));
            Assert.That(result.City, Is.EqualTo(request.City));
            Assert.That(result.Manager, Is.EqualTo(request.Manager));
        }

        [Test]
        public async Task Test_CreateArtGalleryCommand_Handle_ShouldReturnNull_WhenCreateArtGalleryRequestIsNull()
        {
            // Arrange
            var handler = new CreateArtGalleryCommandHandler(NormalArtGalleryRepositoryMock().Object);
            var command = new CreateArtGalleryCommand(null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Test_CreateArtGalleryCommand_Handle_ShouldReturnNull_WhenArgumentNullExceptionThrown()
        {
            // Arrange
            var invalidArtGalleryRepositoryMock = InvalidArtGalleryRepositoryMock();

            var handler = new CreateArtGalleryCommandHandler(invalidArtGalleryRepositoryMock.Object);
            var command = new CreateArtGalleryCommand(null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Null);
        }

        #endregion CreateArtGalleryCommand

        #region UpdateArtGalleryCommand

        [Test]
        public async Task Test_UpdateArtGalleryCommand_Handle_ShouldReturnSaveArtGalleryResult_WhenArtGalleryExists()
        {
            // Arrange
            var command = new UpdateArtGalleryCommand(new UpdateArtGalleryRequest
                (g1.Id,
                "Updated Gallery",
                "Updated City",
                "Updated Manager"));

            var handler = new UpdateArtGalleryCommandHandler(NormalArtGalleryRepositoryMock().Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(g1.Id));
            Assert.That(result.Name, Is.EqualTo("Updated Gallery"));
            Assert.That(result.City, Is.EqualTo("Updated City"));
            Assert.That(result.Manager, Is.EqualTo("Updated Manager"));
        }

        [Test]
        public async Task Test_UpdateArtGalleryCommand_Handle_ShouldReturnNull_WhenArtGalleryDoesNotExist()
        {
            // Arrange
            var command = new UpdateArtGalleryCommand(new UpdateArtGalleryRequest(
                Guid.NewGuid(),
                "Updated Gallery",
                "Updated City",
                "Updated Manager"));

            var handler = new UpdateArtGalleryCommandHandler(NormalArtGalleryRepositoryMock().Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Test_UpdateArtGalleryCommand_Handle_ShouldReturnNull_WhenUpdateArtGalleryRequestIsNull()
        {
            // Arrange
            var command = new UpdateArtGalleryCommand(null);
            var handler = new UpdateArtGalleryCommandHandler(NormalArtGalleryRepositoryMock().Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Null);
        }

        #endregion UpdateArtGalleryCommand

        #region DeleteArtGalleryCommand

        [Test]
        public async Task Test_DeleteArtGalleryCommand_Handle_ShouldReturnTrue_WhenArtGalleryExists()
        {
            // Arrange
            var command = new DeleteArtGalleryCommand(g1.Id);
            var handler = new DeleteArtGalleryCommandHandler(NormalArtGalleryRepositoryMock().Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task Test_DeleteArtGalleryCommand_Handle_ShouldReturnFalse_WhenArtGalleryDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var command = new DeleteArtGalleryCommand(nonExistentId);
            var handler = new DeleteArtGalleryCommandHandler(NormalArtGalleryRepositoryMock().Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task Test_DeleteArtGalleryCommand_Handle_ShouldReturnFalse_WhenArgumentExceptionThrown()
        {
            // Arrange
            var command = new DeleteArtGalleryCommand(g1.Id);
            var handler = new DeleteArtGalleryCommandHandler(InvalidArtGalleryRepositoryMock().Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.False);
        }

        #endregion DeleteArtGalleryCommand

        #region CreateArtWorkCommand

        [Test]
        public async Task Test_CreateArtWorkCommand_Handle_ShouldReturnArtWork_WhenArtWorkIsCreated()
        {
            // Arrange
            var request = new CreateArtWorkRequest("Test Name", "Test Author", 2024, 10000);
            var command = new CreateArtWorkCommand(g1.Id, request);
            var handler = new CreateArtWorkCommandHandler(NormalArtWorksRepositoryMock().Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.Name, Is.EqualTo(request.Name));
            Assert.That(result.Author, Is.EqualTo(request.Author));
            Assert.That(result.CreationYear, Is.EqualTo(request.CreationYear));
            Assert.That(result.AskPrice, Is.EqualTo(request.AskPrice));
        }

        [Test]
        public async Task Test_CreateArtWorkCommand_Handle_ShouldReturnNull_WhenCreateArtWorkRequestIsNull()
        {
            // Arrange
            var command = new CreateArtWorkCommand(g1.Id, null);
            var handler = new CreateArtWorkCommandHandler(NormalArtWorksRepositoryMock().Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Test_CreateArtWorkCommand_Handle_ShouldReturnNull_WhenArgumentExceptionThrown()
        {
            // Arrange
            var command = new CreateArtWorkCommand(Guid.Empty, null);
            var handler = new CreateArtWorkCommandHandler(NormalArtWorksRepositoryMock().Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Null);
        }

        #endregion CreateArtWorkCommand

        #region SetupsAndMocks

        private void SetupGalleriesAndWorks()
        {
            g1.Id = Guid.Parse("7af0ed23-36c1-4097-bae4-525da3b129ce");
            g2.Id = Guid.Parse("c576a9e6-d1ae-4382-98b1-f06de68926a9");

            a1.Id = Guid.Parse("733c9b88-2932-4144-93c6-7e2442ae7d62");
            a1.Id = Guid.Parse("9870e314-296a-4fcd-ab2b-c70fe4c1e820");
            a1.Id = Guid.Parse("48f96312-377f-463c-be4d-154d0cae3e66");
            a1.Id = Guid.Parse("78a15440-f6de-4e86-899c-c1414b1efaaf");
            a1.Id = Guid.Parse("a7714454-09db-4708-834e-f178eecdc44f");

            g1.ArtWorksOnDisplay = new List<ArtWork> { a1, a2 };
            g1.ArtWorksOnDisplay = new List<ArtWork> { a3, a4, a5 };
        }

        private Mock<IArtGalleryRepository> NormalArtGalleryRepositoryMock()
        {
            var mock = new Mock<IArtGalleryRepository>(MockBehavior.Strict);
            mock.Setup(m => m.GetAllArtGalleriesAsync(It.IsAny<CancellationToken>())).ReturnsAsync([g1, g2]);
            mock.Setup(m => m.GetArtGalleryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Guid id, CancellationToken token) => id == g1.Id ? g1 : null);
            mock.Setup(m => m.CreateAsync(It.IsAny<ArtGallery>(), It.IsAny<CancellationToken>())).ReturnsAsync((ArtGallery artGallery, CancellationToken token) =>
                    new ArtGallery(artGallery.Name, artGallery.City, artGallery.Manager) { Id = Guid.NewGuid() });
            mock.Setup(m => m.UpdateAsync(It.IsAny<ArtGallery>(), It.IsAny<CancellationToken>())).ReturnsAsync(g1);
            mock.Setup(m => m.DeleteAsync(g1.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            return mock;
        }

        private Mock<IArtGalleryRepository> InvalidArtGalleryRepositoryMock()
        {
            var mock = new Mock<IArtGalleryRepository>(MockBehavior.Strict);
            mock.Setup(m => m.GetArtGalleryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new ArgumentNullException("artGalleryId"));
            return mock;
        }

        private Mock<IArtWorkRepository> NormalArtWorksRepositoryMock()
        {
            var mock = new Mock<IArtWorkRepository>(MockBehavior.Strict);
            mock.Setup(m => m.GetArtWorksByGalleryIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync([a1, a2]);
            mock.Setup(m => m.GetArtWorkyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Guid id, CancellationToken token) => id == a1.Id ? a1 : null);
            mock.Setup(m => m.CreateAsync(It.IsAny<Guid>(), It.IsAny<ArtWork>(), It.IsAny<CancellationToken>())).ReturnsAsync((Guid id, ArtWork artWork, CancellationToken token) => artWork);

            return mock;
        }

        private Mock<IArtWorkRepository> InvalidGalleryArtWorksRepositoryMock()
        {
            var mock = new Mock<IArtWorkRepository>(MockBehavior.Strict);
            mock.Setup(m => m.GetArtWorksByGalleryIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new ArgumentException("", "artGalleryId"));
            mock.Setup(m => m.GetArtWorkyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new ArgumentNullException("artWorkId"));

            return mock;
        }

        #endregion SetupsAndMocks
    }
}