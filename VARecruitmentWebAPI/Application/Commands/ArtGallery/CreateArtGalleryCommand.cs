using MediatR;
using VAArtGalleryWebAPI.Domain.Entities;
using VAArtGalleryWebAPI.Domain.Interfaces;
using VAArtGalleryWebAPI.WebApi.Models;

namespace VAArtGalleryWebAPI.Application.Commands
{
    public class CreateArtGalleryCommand(CreateArtGalleryRequest createArtGalleryRequest) : IRequest<SaveArtGalleryResult>
    {
        public CreateArtGalleryRequest CreateArtGalleryRequest { get; set; } = createArtGalleryRequest;
    }

    internal sealed class CreateArtGalleryCommandHandler : IRequestHandler<CreateArtGalleryCommand, SaveArtGalleryResult?>
    {
        private readonly IArtGalleryRepository _artGalleryRepository;

        public CreateArtGalleryCommandHandler(IArtGalleryRepository artGalleryRepository)
        {
            _artGalleryRepository = artGalleryRepository ?? throw new ArgumentNullException(nameof(artGalleryRepository));
        }

        public async Task<SaveArtGalleryResult?> Handle(CreateArtGalleryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.CreateArtGalleryRequest is null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var artGallery = new ArtGallery(
                    request.CreateArtGalleryRequest.Name,
                    request.CreateArtGalleryRequest.City,
                    request.CreateArtGalleryRequest.Manager);

                var result = await _artGalleryRepository.CreateAsync(artGallery, cancellationToken);

                return new SaveArtGalleryResult(result.Id, result.Name, result.City, result.Manager);
            }
            catch (ArgumentNullException ex) when (ex.ParamName == nameof(request))
            {
                return null;
            }
        }
    }
}