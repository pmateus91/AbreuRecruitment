using MediatR;
using System.Runtime.CompilerServices;
using VAArtGalleryWebAPI.Domain.Interfaces;
using VAArtGalleryWebAPI.WebApi.Models;

[assembly: InternalsVisibleTo("VAArtGalleryWebAPITest")]

namespace VAArtGalleryWebAPI.Application.Queries
{
    public class GetArtGalleryByIdQuery(Guid artGalleryId) : IRequest<GetArtGalleryResult?>
    {
        public Guid ArtGalleryId { get; set; } = artGalleryId;
    }

    internal sealed class GetArtGalleryByIdQueryHandler : IRequestHandler<GetArtGalleryByIdQuery, GetArtGalleryResult?>
    {
        private readonly IArtGalleryRepository _artGalleryRepository;

        public GetArtGalleryByIdQueryHandler(IArtGalleryRepository artGalleryRepository)
        {
            _artGalleryRepository = artGalleryRepository ?? throw new ArgumentNullException(nameof(artGalleryRepository));
        }

        public async Task<GetArtGalleryResult?> Handle(GetArtGalleryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _artGalleryRepository.GetArtGalleryByIdAsync(request.ArtGalleryId, cancellationToken);
                if (result is null)
                {
                    return null;
                }

                return new GetArtGalleryResult(result.Id, result.Name, result.City, result.Manager, result.ArtWorksOnDisplay?.Count ?? 0);
            }
            catch (ArgumentNullException ex) when (string.Compare(ex.ParamName, "artGalleryId", true) == 0)
            {
                return null;
            }
        }
    }
}