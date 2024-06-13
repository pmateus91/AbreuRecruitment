using MediatR;
using System.Runtime.CompilerServices;
using VAArtGalleryWebAPI.Domain.Interfaces;
using VAArtGalleryWebAPI.WebApi.Models;

[assembly: InternalsVisibleTo("VAArtGalleryWebAPITest")]

namespace VAArtGalleryWebAPI.Application.Queries
{
    public class GetArtWorkByIdQuery(Guid artWorkId) : IRequest<GetArtWorkResult?>
    {
        public Guid ArtWorkId { get; set; } = artWorkId;
    }

    internal sealed class GetArtWorkByIdQueryHandler : IRequestHandler<GetArtWorkByIdQuery, GetArtWorkResult?>
    {
        private readonly IArtWorkRepository _artWorkRepository;

        public GetArtWorkByIdQueryHandler(IArtWorkRepository artWorkRepository)
        {
            _artWorkRepository = artWorkRepository ?? throw new ArgumentNullException(nameof(artWorkRepository));
        }

        public async Task<GetArtWorkResult?> Handle(GetArtWorkByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _artWorkRepository.GetArtWorkyByIdAsync(request.ArtWorkId, cancellationToken);
                if (result is null)
                {
                    return null;
                }

                return new GetArtWorkResult(result.Id, result.Name, result.Author, result.CreationYear, result.AskPrice);
            }
            catch (ArgumentNullException ex) when (string.Compare(ex.ParamName, "artWorkId", true) == 0)
            {
                return null;
            }
        }
    }
}