using MediatR;
using VAArtGalleryWebAPI.Domain.Interfaces;
using VAArtGalleryWebAPI.WebApi.Models;

namespace VAArtGalleryWebAPI.Application.Commands
{
    public class UpdateArtWorkCommand(Guid artGalleryId, UpdateArtWorkRequest updateArtWorkRequest) : IRequest<SaveArtWorkResult?>
    {
        public Guid ArtGalleryId { get; set; } = artGalleryId;
        public UpdateArtWorkRequest UpdateArtWorkRequest { get; set; } = updateArtWorkRequest;
    }

    internal sealed class UpdateArtWorkCommandHandler : IRequestHandler<UpdateArtWorkCommand, SaveArtWorkResult?>
    {
        private readonly IArtWorkRepository _artWorkRepository;

        public UpdateArtWorkCommandHandler(IArtWorkRepository artWorkRepository)
        {
            _artWorkRepository = artWorkRepository ?? throw new ArgumentNullException(nameof(artWorkRepository));
        }

        public async Task<SaveArtWorkResult?> Handle(UpdateArtWorkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.UpdateArtWorkRequest is null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var artWork = await _artWorkRepository.GetArtWorkyByIdAsync(request.UpdateArtWorkRequest.Id, cancellationToken);
                if (artWork is null)
                {
                    return null;
                }

                artWork.Update(request.UpdateArtWorkRequest.Name, request.UpdateArtWorkRequest.Author, request.UpdateArtWorkRequest.CreationYear, request.UpdateArtWorkRequest.AskPrice);

                var result = await _artWorkRepository.UpdateAsync(request.ArtGalleryId, artWork, cancellationToken);

                return new SaveArtWorkResult(result.Id, result.Name, result.Author, result.CreationYear, result.AskPrice);
            }
            catch (ArgumentNullException ex) when (string.Compare(ex.ParamName, "artGalleryId", true) == 0 || ex.ParamName == nameof(request))
            {
                return null;
            }
        }
    }
}