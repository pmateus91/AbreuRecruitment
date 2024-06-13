using MediatR;
using VAArtGalleryWebAPI.Domain.Interfaces;
using VAArtGalleryWebAPI.WebApi.Models;

namespace VAArtGalleryWebAPI.Application.Commands
{
    public class UpdateArtGalleryCommand(UpdateArtGalleryRequest updateArtGalleryRequest) : IRequest<SaveArtGalleryResult>
    {
        public UpdateArtGalleryRequest UpdateArtGalleryRequest { get; set; } = updateArtGalleryRequest;
    }

    internal sealed class UpdateArtGalleryCommandHandler : IRequestHandler<UpdateArtGalleryCommand, SaveArtGalleryResult?>
    {
        private readonly IArtGalleryRepository _artGalleryRepository;

        public UpdateArtGalleryCommandHandler(IArtGalleryRepository artGalleryRepository)
        {
            _artGalleryRepository = artGalleryRepository ?? throw new ArgumentNullException(nameof(artGalleryRepository));
        }

        public async Task<SaveArtGalleryResult?> Handle(UpdateArtGalleryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.UpdateArtGalleryRequest is null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var artGallery = await _artGalleryRepository.GetArtGalleryByIdAsync(request.UpdateArtGalleryRequest.Id, cancellationToken);
                if (artGallery is null)
                {
                    return null;
                }

                artGallery.Update(request.UpdateArtGalleryRequest.Name, request.UpdateArtGalleryRequest.City, request.UpdateArtGalleryRequest.Manager);

                var result = await _artGalleryRepository.UpdateAsync(artGallery, cancellationToken);

                return new SaveArtGalleryResult(result.Id, result.Name, result.City, result.Manager);
            }
            catch (ArgumentNullException ex) when (ex.ParamName == nameof(request))
            {
                return null;
            }
        }
    }
}