using VAArtGalleryWebAPI.Domain.Entities;

namespace VAArtGalleryWebAPI.Domain.Interfaces
{
    public interface IArtWorkRepository
    {
        Task<List<ArtWork>> GetArtWorksByGalleryIdAsync(Guid artGalleryId, CancellationToken cancellationToken = default);

        Task<ArtWork?> GetArtWorkyByIdAsync(Guid artWorkId, CancellationToken cancellationToken = default);

        Task<ArtWork> CreateAsync(Guid artGalleryId, ArtWork artWork, CancellationToken cancellationToken = default);

        Task<ArtWork> UpdateAsync(Guid artGalleryId, ArtWork artWork, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(Guid artWorkId, CancellationToken cancellationToken = default);
    }
}