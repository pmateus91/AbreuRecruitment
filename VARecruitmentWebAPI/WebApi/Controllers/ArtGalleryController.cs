using MediatR;
using Microsoft.AspNetCore.Mvc;
using VAArtGalleryWebAPI.Application.Commands;
using VAArtGalleryWebAPI.Application.Queries;
using VAArtGalleryWebAPI.WebApi.Models;

namespace VAArtGalleryWebAPI.WebApi.Controllers
{
    /// <summary>
    /// Controller for managing Art Galleries.
    /// </summary>
    [Route("api/art-galleries")]
    [ApiController]
    public class ArtGalleryController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Gets all art galleries.
        /// </summary>
        /// <returns>List of art galleries.</returns>
        [HttpGet]
        [Route("GetAllGalleries")]
        public async Task<ActionResult<List<GetArtGalleryResult>>> GetAllGalleries()
        {
            var galleries = await mediator.Send(new GetAllArtGalleriesQuery());

            var result = galleries.Select(g => new GetArtGalleryResult(g.Id, g.Name, g.City, g.Manager, g.ArtWorksOnDisplay?.Count ?? 0)).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Gets an art gallery by its ID.
        /// </summary>
        /// <param name="artGalleryId">The ID of the art gallery.</param>
        /// <returns>The requested art gallery.</returns>
        [HttpGet]
        [Route("GetArtGalleryById")]
        public async Task<ActionResult<GetArtGalleryResult>> GetArtGalleryByIdAsync(Guid artGalleryId)
        {
            var result = await mediator.Send(new GetArtGalleryByIdQuery(artGalleryId));
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets all ArtWorks in a specific art gallery.
        /// </summary>
        /// <param name="artGalleryId">The ID of the art gallery.</param>
        /// <returns>List of artworks in the specified art gallery.</returns>
        [HttpGet]
        [Route("GetArtGalleryArtWorks")]
        public async Task<ActionResult<IEnumerable<GetArtWorkResult>>> GetArtGalleryArtWorksAsync(Guid artGalleryId)
        {
            var result = await mediator.Send(new GetArtGalleryArtWorksQuery(artGalleryId));
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Creates a new art gallery.
        /// </summary>
        /// <param name="request">The request object containing the details of the new art gallery.</param>
        /// <returns>The created art gallery.</returns>
        [HttpPost]
        public async Task<ActionResult<SaveArtGalleryResult>> CreateArtGalleryAsync([FromBody] CreateArtGalleryRequest request)
        {
            var result = await mediator.Send(new CreateArtGalleryCommand(request));
            if (result is null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        /// <summary>
        /// Updates an existing art gallery.
        /// </summary>
        /// <param name="request">The request object containing the updated details of the art gallery.</param>
        /// <returns>The updated art gallery.</returns>
        [HttpPut]
        public async Task<ActionResult<SaveArtGalleryResult>> UpdateArtGalleryAsync([FromBody] UpdateArtGalleryRequest request)
        {
            var result = await mediator.Send(new UpdateArtGalleryCommand(request));
            if (result is null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        /// <summary>
        /// Deletes an art gallery.
        /// </summary>
        /// <param name="artGalleryId">The ID of the art gallery to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteArtGalleryAsync(Guid artGalleryId)
        {
            var result = await mediator.Send(new DeleteArtGalleryCommand(artGalleryId));
            if (!result)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}