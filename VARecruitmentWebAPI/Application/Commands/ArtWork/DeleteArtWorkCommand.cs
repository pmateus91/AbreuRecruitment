using MediatR;
using VAArtGalleryWebAPI.Domain.Interfaces;

namespace VAArtGalleryWebAPI.Application.Commands
{
    public class DeleteArtWorkCommand(Guid artWorkId) : IRequest<bool>
    {
        public Guid ArtWorkId { get; set; } = artWorkId;
    }

    internal sealed class DeleteArtWorkCommandHandler : IRequestHandler<DeleteArtWorkCommand, bool>
    {
        private readonly IArtWorkRepository _artWorkRepository;

        public DeleteArtWorkCommandHandler(IArtWorkRepository artWorkRepository)
        {
            _artWorkRepository = artWorkRepository ?? throw new ArgumentNullException(nameof(artWorkRepository));
        }

        public async Task<bool> Handle(DeleteArtWorkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var artWork = await _artWorkRepository.GetArtWorkyByIdAsync(request.ArtWorkId, cancellationToken);
                if (artWork is null)
                {
                    return false;
                }

                return await _artWorkRepository.DeleteAsync(request.ArtWorkId, cancellationToken);
            }
            catch (ArgumentException ex) when (string.Compare(ex.ParamName, "artWorkId", true) == 0)
            {
                return false;
            }
        }
    }
}