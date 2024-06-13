using System.Text.Json;
using VAArtGalleryWebAPI.Domain.Interfaces;
using VAArtGalleryWebAPI.Domain.Entities;

namespace VAArtGalleryWebAPI.Infrastructure
{
    public class ArtWorkRepository(string filePath) : IArtWorkRepository
    {
        private readonly string _filePath = filePath;

        public async Task<List<ArtWork>> GetArtWorksByGalleryIdAsync(Guid artGalleryId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var galleries = await new ArtGalleryRepository(_filePath).GetAllArtGalleriesAsync(cancellationToken);

            var gallery = galleries.Find(g => g.Id == artGalleryId) ?? throw new ArgumentException("unknown art gallery", nameof(artGalleryId));
            if (gallery.ArtWorksOnDisplay == null)
            {
                return [];
            }
            return gallery.ArtWorksOnDisplay;
        }

        public async Task<ArtWork?> GetArtWorkyByIdAsync(Guid artWorkId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var galleries = await new ArtGalleryRepository(_filePath).GetAllArtGalleriesAsync(cancellationToken);

            return galleries
                .SelectMany(g => g.ArtWorksOnDisplay ?? Enumerable.Empty<ArtWork>())
                .FirstOrDefault(aw => aw.Id == artWorkId);
        }

        public async Task<ArtWork> CreateAsync(Guid artGalleryId, ArtWork artWork, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var galleries = await new ArtGalleryRepository(_filePath).GetAllArtGalleriesAsync(cancellationToken);

            var gallery = galleries.Find(g => g.Id == artGalleryId) ?? throw new ArgumentException("unknown art gallery", nameof(artGalleryId));
            artWork.Id = Guid.NewGuid();

            if (gallery.ArtWorksOnDisplay == null)
            {
                gallery.ArtWorksOnDisplay = [artWork];
            }
            else
            {
                gallery.ArtWorksOnDisplay.Add(artWork);
            }

            cancellationToken.ThrowIfCancellationRequested();

            await SaveGalleries(galleries);

            return artWork;
        }

        public async Task<ArtWork> UpdateAsync(Guid artGalleryId, ArtWork artWork, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var galleries = await new ArtGalleryRepository(_filePath).GetAllArtGalleriesAsync(cancellationToken);

            var gallery = galleries.Find(g => g.Id == artGalleryId) ?? throw new ArgumentException("Unknown art gallery", nameof(artGalleryId));

            var artWorkToUpdate = gallery.ArtWorksOnDisplay?.Find(aw => aw.Id == artWork.Id) ?? throw new ArgumentException("Unknown art work", nameof(artWork));

            artWorkToUpdate.Update(artWork.Name, artWork.Author, artWork.CreationYear, artWork.AskPrice);

            cancellationToken.ThrowIfCancellationRequested();

            await SaveGalleries(galleries);

            return artWork;
        }

        public async Task<bool> DeleteAsync(Guid artWorkId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var galleries = await new ArtGalleryRepository(_filePath).GetAllArtGalleriesAsync(cancellationToken);

            var isArtWorkDeleted = galleries
                .Select(g => g.ArtWorksOnDisplay)
                .Select(artWorks =>
                {
                    if (artWorks is null || artWorks.Count == 0)
                    {
                        return false;
                    }

                    var artworkToRemove = artWorks.Find(aw => aw.Id == artWorkId);
                    if (artworkToRemove is not null)
                    {
                        artWorks.Remove(artworkToRemove);
                        return true;
                    }

                    return false;
                })
                .Any(deleted => deleted);

            if (!isArtWorkDeleted)
            {
                return false;
            }

            cancellationToken.ThrowIfCancellationRequested();

            await SaveGalleries(galleries);

            return true;
        }

        private async Task SaveGalleries(List<ArtGallery> galleries)
        {
            using TextWriter tw = new StreamWriter(_filePath, false);
            await tw.WriteAsync(JsonSerializer.Serialize(galleries));
        }
    }
}