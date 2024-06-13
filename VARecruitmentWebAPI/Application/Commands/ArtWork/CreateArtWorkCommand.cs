using MediatR;
using VAArtGalleryWebAPI.Domain.Entities;
using VAArtGalleryWebAPI.Domain.Interfaces;
using VAArtGalleryWebAPI.WebApi.Models;

namespace VAArtGalleryWebAPI.Application.Commands
{
    public class CreateArtWorkCommand(Guid artGalleryId, CreateArtWorkRequest createArtWorkRequest) : IRequest<SaveArtWorkResult?>
    {
        public Guid ArtGalleryId { get; set; } = artGalleryId;
        public CreateArtWorkRequest CreateArtWorkRequest { get; set; } = createArtWorkRequest;
    }

    internal sealed class CreateArtWorkCommandHandler : IRequestHandler<CreateArtWorkCommand, SaveArtWorkResult?>
    {
        private readonly IArtWorkRepository _artWorkRepository;

        public CreateArtWorkCommandHandler(IArtWorkRepository artWorkRepository)
        {
            _artWorkRepository = artWorkRepository ?? throw new ArgumentNullException(nameof(artWorkRepository));
        }

        public async Task<SaveArtWorkResult?> Handle(CreateArtWorkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.CreateArtWorkRequest is null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var artWork = new ArtWork(
                    request.CreateArtWorkRequest.Name,
                    request.CreateArtWorkRequest.Author,
                    request.CreateArtWorkRequest.CreationYear,
                    request.CreateArtWorkRequest.AskPrice);

                var result = await _artWorkRepository.CreateAsync(request.ArtGalleryId, artWork, cancellationToken);

                return new SaveArtWorkResult(result.Id, result.Name, result.Author, result.CreationYear, result.AskPrice);
            }
            catch (ArgumentNullException ex) when (string.Compare(ex.ParamName, "artGalleryId", true) == 0 || ex.ParamName == nameof(request))
            {
                return null;
            }
        }
    }
}