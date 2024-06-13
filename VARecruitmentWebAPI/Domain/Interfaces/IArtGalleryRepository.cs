using VAArtGalleryWebAPI.Domain.Entities;

namespace VAArtGalleryWebAPI.Domain.Interfaces
{
    public interface IArtGalleryRepository
    {
        Task<List<ArtGallery>> GetAllArtGalleriesAsync(CancellationToken cancellationToken = default);

        Task<ArtGallery?> GetArtGalleryByIdAsync(Guid artGalleryId, CancellationToken cancellationToken = default);

        Task<ArtGallery> CreateAsync(ArtGallery artGallery, CancellationToken cancellationToken = default);

        Task<ArtGallery> UpdateAsync(ArtGallery artGallery, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(Guid artGalleryId, CancellationToken cancellationToken = default);
    }
}