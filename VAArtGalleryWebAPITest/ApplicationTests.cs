using Moq;
using VAArtGalleryWebAPI.Application.Queries;
using VAArtGalleryWebAPI.Domain.Entities;
using VAArtGalleryWebAPI.Domain.Interfaces;

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

            return mock;
        }

        private Mock<IArtWorkRepository> NormalArtWorksRepositoryMock()
        {
            var mock = new Mock<IArtWorkRepository>(MockBehavior.Strict);
            mock.Setup(m => m.GetArtWorksByGalleryIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync([a1, a2]);
            mock.Setup(m => m.GetArtWorkyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Guid id, CancellationToken token) => id == a1.Id ? a1 : null);

            return mock;
        }

        private Mock<IArtWorkRepository> InvalidGalleryArtWorksRepositoryMock()
        {
            var mock = new Mock<IArtWorkRepository>(MockBehavior.Strict);
            mock.Setup(m => m.GetArtWorksByGalleryIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new ArgumentException("", "artGalleryId"));
            mock.Setup(m => m.GetArtWorkyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new ArgumentNullException("artWorkId"));

            return mock;
        }
    }
}