using Application.Invoices.Commands;
using Application.Invoices.Queries;
using InkpactAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InkpactAPI.Controllers
{
    [ApiController]
    [Route("api/invoices")]
    public class InvoiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get all invoices for a contract. Both contract participants can view.
        [HttpGet("contract/{contractId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetForContract([FromRoute] Guid contractId)
        {
            var result = await _mediator.Send(
                new GetInvoicesForContractQuery(contractId, User.GetUserId()));
            return result.ToActionResult();
        }

        // Mark an invoice as paid. Only the client of the contract can do this.
        [HttpPatch("{id:guid}/pay")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> MarkPaid([FromRoute] Guid id)
        {
            var result = await _mediator.Send(
                new MarkInvoicePaidCommand(id, User.GetUserId()));
            return result.ToActionResult();
        }
    }
}
