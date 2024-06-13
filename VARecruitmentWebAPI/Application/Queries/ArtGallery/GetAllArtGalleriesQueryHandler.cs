using MediatR;
using VAArtGalleryWebAPI.Domain.Entities;
using VAArtGalleryWebAPI.Domain.Interfaces;

namespace VAArtGalleryWebAPI.Application.Queries
{
    public class GetAllArtGalleriesQueryHandler(IArtGalleryRepository artGalleryRepository) : IRequestHandler<GetAllArtGalleriesQuery, List<ArtGallery>>
    {
        public async Task<List<ArtGallery>> Handle(GetAllArtGalleriesQuery request, CancellationToken cancellationToken)
        {
            return await artGalleryRepository.GetAllArtGalleriesAsync(cancellationToken);
        }
    }
}
