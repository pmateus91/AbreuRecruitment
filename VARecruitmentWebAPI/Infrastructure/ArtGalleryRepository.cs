using System.Text.Json;
using VAArtGalleryWebAPI.Domain.Entities;
using VAArtGalleryWebAPI.Domain.Interfaces;

namespace VAArtGalleryWebAPI.Infrastructure
{
    public class ArtGalleryRepository(string filePath) : IArtGalleryRepository
    {
        private readonly string _filePath = filePath;

        public async Task<List<ArtGallery>> GetAllArtGalleriesAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                using StreamReader sr = new(_filePath);
                string galleriesJson = sr.ReadToEnd();
                return JsonSerializer.Deserialize<List<ArtGallery>>(galleriesJson) ?? [];
            });
        }

        public async Task<ArtGallery?> GetArtGalleryByIdAsync(Guid artGalleryId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var galleries = await GetAllArtGalleriesAsync(cancellationToken);
            return galleries.Find(g => g.Id == artGalleryId);
        }

        public async Task<ArtGallery> CreateAsync(ArtGallery artGallery, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var galleries = await GetAllArtGalleriesAsync(cancellationToken);

            artGallery.Id = Guid.NewGuid();
            galleries.Add(artGallery);

            await SaveGalleries(galleries);

            return artGallery;
        }

        public async Task<ArtGallery> UpdateAsync(ArtGallery artGallery, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var galleries = await GetAllArtGalleriesAsync(cancellationToken);
            var galleryToUpdate = galleries.Find(g => g.Id == artGallery.Id) ?? throw new ArgumentException("unknown art gallery", nameof(artGallery));

            galleryToUpdate.Update(artGallery.Name, artGallery.City, artGallery.Manager);

            cancellationToken.ThrowIfCancellationRequested();

            await SaveGalleries(galleries);

            return artGallery;
        }

        public async Task<bool> DeleteAsync(Guid artGalleryId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var galleries = await GetAllArtGalleriesAsync(cancellationToken);

            var artGalleryToRemove = galleries.Find(g => g.Id == artGalleryId) ?? throw new ArgumentException("unknown art gallery", nameof(artGalleryId));

            galleries.Remove(artGalleryToRemove);

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