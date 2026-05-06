using Application.Proposals.Commands.AcceptProposal;
using Application.Proposals.Commands.RejectProposal;
using Application.Proposals.Commands.SubmitProposal;
using Application.Proposals.Commands.WithdrawProposal;
using Application.Proposals.Queries.GetMyProposals;
using Application.Proposals.Queries.GetProposalForGig;
using InkpactAPI.Common;
using InkpactAPI.Requests.Proposals;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InkpactAPI.Controllers;

[ApiController]
[Route("api/proposals")]
public class ProposalsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProposalsController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Get all proposals submitted by the currently logged-in freelancer.
    /// </summary>
    [HttpGet("me")]
    [Authorize(Roles = "Freelancer")]
    public async Task<IActionResult> GetMyProposals()
    {
        var result = await _mediator.Send(new GetMyProposalsQuery(User.GetUserId()));
        return result.ToActionResult();
    }

    /// <summary>
    /// Get all proposals for a specific gig. Only the gig owner can view these.
    /// </summary>
    [HttpGet("gig/{gigId:guid}")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetForGig([FromRoute] Guid gigId)
    {
        var result = await _mediator.Send(
            new GetProposalsForGigQuery(gigId, User.GetUserId()));
        return result.ToActionResult();
    }

    /// <summary>
    /// Submit a proposal to a gig. Freelancers only.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Freelancer")]
    public async Task<IActionResult> Submit([FromBody] SubmitProposalRequest request)
    {
        var command = new SubmitProposalCommand(
            request.GigId,
            User.GetUserId(),
            request.CoverLetter,
            request.ProposedRate,
            request.EstimatedDays
        );

        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }

    /// <summary>
    /// Accept a proposal. Triggers contract creation and rejects all other proposals on the same gig.
    /// </summary>
    [HttpPatch("{id:guid}/accept")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Accept([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new AcceptProposalCommand(id, User.GetUserId()));
        return result.ToActionResult();
    }

    /// <summary>
    /// Reject a single proposal. Only the gig owner can reject.
    /// </summary>
    [HttpPatch("{id:guid}/reject")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Reject([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new RejectProposalCommand(id, User.GetUserId()));
        return result.ToActionResult();
    }

    /// <summary>
    /// Withdraw your own proposal. Only the freelancer who submitted it can withdraw.
    /// </summary>
    [HttpPatch("{id:guid}/withdraw")]
    [Authorize(Roles = "Freelancer")]
    public async Task<IActionResult> Withdraw([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new WithdrawProposalCommand(id, User.GetUserId()));
        return result.ToActionResult();
    }
}
