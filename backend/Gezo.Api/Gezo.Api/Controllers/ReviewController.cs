using Gezo.Application.Features.Reviews.Commands.AddReviewCommand;
using Gezo.Application.Features.Reviews.Queries.GetProductReviewsQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gezo.Api.Controllers
{
    [Route("api/Product/{productId}/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetProductReviews(int productId)
        {
            var result = await _mediator.Send(new GetProductReviewsQuery(productId));
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddReview(int productId, AddReviewCommand command)
        {
            command.ProductId = productId;
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}

