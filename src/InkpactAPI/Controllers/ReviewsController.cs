using Application.Reviews.Commands.LeaveReview;
using Application.Reviews.Queries.GetReviewForFreeLancer;
using InkpactAPI.Common;
using InkpactAPI.Requests.Reviews;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InkpactAPI.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewsController(IMediator mediator) => _mediator = mediator;

        [HttpGet("freelancer/{freelancerId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetForFreelancer([FromRoute] Guid freelancerId)
        {
            var result = await _mediator.Send(
                new GetReviewsForFreelancerQuery(freelancerId));
            return result.ToActionResult();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Leave([FromBody] LeaveReviewRequest request)
        {
            var command = new LeaveReviewCommand(
                request.ContractId,
                User.GetUserId(),
                request.Rating,
                request.Comment);

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
