using MediatR;
using VAArtGalleryWebAPI.Domain.Interfaces;

namespace VAArtGalleryWebAPI.Application.Commands
{
    public class DeleteArtGalleryCommand(Guid artGalleryId) : IRequest<bool>
    {
        public Guid ArtGalleryId { get; set; } = artGalleryId;
    }

    internal sealed class DeleteArtGalleryCommandHandler : IRequestHandler<DeleteArtGalleryCommand, bool>
    {
        private readonly IArtGalleryRepository _artGalleryRepository;

        public DeleteArtGalleryCommandHandler(IArtGalleryRepository artGalleryRepository)
        {
            _artGalleryRepository = artGalleryRepository ?? throw new ArgumentNullException(nameof(artGalleryRepository));
        }

        public async Task<bool> Handle(DeleteArtGalleryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var artGallery = await _artGalleryRepository.GetArtGalleryByIdAsync(request.ArtGalleryId, cancellationToken);
                if (artGallery is null)
                {
                    return false;
                }

                return await _artGalleryRepository.DeleteAsync(request.ArtGalleryId, cancellationToken);
            }
            catch (ArgumentException ex) when (string.Compare(ex.ParamName, "artGalleryId", true) == 0)
            {
                return false;
            }
        }
    }
}