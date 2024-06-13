using MediatR;
using VAArtGalleryWebAPI.Domain.Entities;

namespace VAArtGalleryWebAPI.Application.Queries
{
    public class GetArtGalleryArtWorksQuery(Guid galleryId) : IRequest<List<ArtWork>>
    {
        public Guid GalleryId { get; set; } = galleryId;
    }
}