using Application.Gigs.Commands.CancelGig;
using Application.Gigs.Commands.CloseGig;
using Application.Gigs.Commands.EditGig;
using Application.Gigs.Commands.PostGig;
using Application.Gigs.Queries.GetGigById;
using Application.Gigs.Queries.GetMyPostedGigs;
using Application.Gigs.Queries.SearchGigs;
using InkpactAPI.Common;
using InkpactAPI.Requests.Gigs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InkpactAPI.Controllers
{
    [ApiController]
    [Route("api/gigs")]
    public class GigsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GigsController(IMediator mediator) => _mediator = mediator;

        /// Search gigs publicly. Anyone can browse.
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search(
            [FromQuery] string? tag,
            [FromQuery] decimal? maxBudget,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new SearchGigsQuery(tag, maxBudget, page, pageSize));
            return result.ToActionResult();
        }

        /// Get a single gig by ID. Public.
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetGigByIdQuery(id));
            return result.ToActionResult();
        }

        /// Get all gigs posted by the currently logged-in client.
        [HttpGet("me")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetMyPostedGigs()
        {
            var result = await _mediator.Send(new GetMyPostedGigsQuery(User.GetUserId()));
            return result.ToActionResult();
        }

        /// Post a new gig. Only clients can create gigs.
        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Post([FromBody] PostGigRequest request)
        {
            var command = new PostGigCommand(
                request.Title,
                request.Description,
                request.Budget,
                request.Deadline,
                request.Tags,
                User.GetUserId()
            );

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        /// Edit an existing gig. Only the owner can edit, only while it's still Open.
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Edit(
            [FromRoute] Guid id,
            [FromBody] EditGigRequest request)
        {
            var command = new EditGigCommand(
                id,
                User.GetUserId(),
                request.Title,
                request.Description,
                request.Budget,
                request.Deadline,
                request.Tags
            );

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        /// Close a gig (no longer accepting proposals). Only the owner.
        [HttpPatch("{id:guid}/close")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Close([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new CloseGigCommand(id, User.GetUserId()));
            return result.ToActionResult();
        }

        /// Cancel a gig entirely. Only the owner.
        [HttpPatch("{id:guid}/cancel")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Cancel([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new CancelGigCommand(id, User.GetUserId()));
            return result.ToActionResult();
        }
    }
}