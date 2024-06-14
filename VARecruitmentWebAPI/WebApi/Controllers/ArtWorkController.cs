using MediatR;
using Microsoft.AspNetCore.Mvc;
using VAArtGalleryWebAPI.Application.Commands;
using VAArtGalleryWebAPI.Application.Queries;
using VAArtGalleryWebAPI.WebApi.Models;

namespace VAArtGalleryWebAPI.WebApi.Controllers
{
    /// <summary>
    /// Controller for managing art works.
    /// </summary>
    [Route("api/art-works")]
    [ApiController]
    public class ArtWorkController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Gets an art work by its ID.
        /// </summary>
        /// <param name="artWorkId">The ID of the art work.</param>
        /// <returns>The requested art work.</returns>
        [HttpGet]
        [Route("GetArtWorkById")]
        public async Task<ActionResult<GetArtWorkResult>> GetArtWorkByIdAsync(Guid artWorkId)
        {
            var result = await mediator.Send(new GetArtWorkByIdQuery(artWorkId));
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Creates a new art work.
        /// </summary>
        /// <param name="artGalleryId">The ID of the art gallery to which the art work belongs.</param>
        /// <param name="request">The request object containing the details of the new art work.</param>
        /// <returns>The created art work.</returns>
        [HttpPost]
        public async Task<ActionResult<SaveArtWorkResult>> CreateArtWorkAsync(Guid artGalleryId, [FromBody] CreateArtWorkRequest request)
        {
            var result = await mediator.Send(new CreateArtWorkCommand(artGalleryId, request));
            if (result is null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        /// <summary>
        /// Updates an existing art work.
        /// </summary>
        /// <param name="artGalleryId">The ID of the art gallery to which the art work belongs.</param>
        /// <param name="request">The request object containing the updated details of the art work.</param>
        /// <returns>The updated art work.</returns>
        [HttpPut]
        public async Task<ActionResult<SaveArtWorkResult>> UpdateArtWorkAsync(Guid artGalleryId, [FromBody] UpdateArtWorkRequest request)
        {
            var result = await mediator.Send(new UpdateArtWorkCommand(artGalleryId, request));
            if (result is null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        /// <summary>
        /// Deletes an art work.
        /// </summary>
        /// <param name="artWorkId">The ID of the art work to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteArtWorkAsync(Guid artWorkId)
        {
            var result = await mediator.Send(new DeleteArtWorkCommand(artWorkId));
            if (!result)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}