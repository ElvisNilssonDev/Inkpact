using Application.Milestones.Commands.AddMilestone;
using Application.Milestones.Commands.ApproveMilestone;
using Application.Milestones.Commands.SubmitMilestone;
using Application.Milestones.Queries.GetMilestonesForContract;
using InkpactAPI.Common;
using InkpactAPI.Requests.Milestones;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InkpactAPI.Controllers
{
    [ApiController]
    [Route("api/milestones")]
    [Authorize]
    public class MilestonesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MilestonesController(IMediator mediator) => _mediator = mediator;

        [HttpGet("contract/{contractId:guid}")]
        public async Task<IActionResult> GetForContract([FromRoute] Guid contractId)
        {
            var result = await _mediator.Send(
                new GetMilestonesForContractQuery(contractId, User.GetUserId()));
            return result.ToActionResult();
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Add([FromBody] AddMilestoneRequest request)
        {
            var command = new AddMilestoneCommand(
                request.ContractId,
                User.GetUserId(),
                request.Title,
                request.Description,
                request.Amount,
                request.DueDate);

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPatch("{id:guid}/submit")]
        [Authorize(Roles = "Freelancer")]
        public async Task<IActionResult> Submit([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new SubmitMilestoneCommand(id, User.GetUserId()));
            return result.ToActionResult();
        }

        [HttpPatch("{id:guid}/approve")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Approve([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new ApproveMilestoneCommand(id, User.GetUserId()));
            return result.ToActionResult();
        }
    }

}
