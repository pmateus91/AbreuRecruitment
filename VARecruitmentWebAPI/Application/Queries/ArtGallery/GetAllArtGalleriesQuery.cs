using MediatR;
using VAArtGalleryWebAPI.Domain.Entities;

namespace VAArtGalleryWebAPI.Application.Queries
{
    public class GetAllArtGalleriesQuery : IRequest<List<ArtGallery>>
    {
    }
}
