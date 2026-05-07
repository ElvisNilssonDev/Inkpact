using Application.Contracts.Commands.CompleteContract;
using Application.Contracts.Commands.TerminateContract;
using Application.Contracts.Queries.GetContractById;
using Application.Contracts.Queries.GetMyContracts;
using Domain.Entities;
using InkpactAPI.Requests.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InkpactAPI.Common;


namespace InkpactAPI.Controllers
{
    [ApiController]
    [Route("api/contracts")]
    [Authorize]
    public class ContractsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContractsController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Get all contracts for the currently logged-in user (works for both clients and freelancers).
        /// </summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetMine()
        {
            var result = await _mediator.Send(new GetMyContractsQuery(User.GetUserId()));
            return result.ToActionResult();
        }

        /// <summary>
        /// Get a contract by ID. Only the contract participants can view it.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetContractByIdQuery(id, User.GetUserId()));
            return result.ToActionResult();
        }

        /// <summary>
        /// Mark a contract as completed. Either party can complete an active contract.
        /// </summary>
        [HttpPatch("{id:guid}/complete")]
        public async Task<IActionResult> Complete([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new CompleteContractCommand(id, User.GetUserId()));
            return result.ToActionResult();
        }

        /// <summary>
        /// Terminate a contract early. Requires a reason for the audit trail.
        /// </summary>
        [HttpPatch("{id:guid}/terminate")]
        public async Task<IActionResult> Terminate(
            [FromRoute] Guid id,
            [FromBody] TerminateContractRequest request)
        {
            var command = new TerminateContractCommand(id, User.GetUserId(), request.Reason);
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
