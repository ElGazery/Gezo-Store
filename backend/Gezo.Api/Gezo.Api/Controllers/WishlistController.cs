using Gezo.Application.Features.Wishlist.Commands.AddToWishlistCommand;
using Gezo.Application.Features.Wishlist.Commands.RemoveFromWishlistCommand;
using Gezo.Application.Features.Wishlist.Queries.GetWishlistQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gezo.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WishlistController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetWishlist()
        {
            var result = await _mediator.Send(new GetWishlistQuery());
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{productId}")]
        public async Task<ActionResult> AddToWishlist(int productId)
        {
            var result = await _mediator.Send(new AddToWishlistCommand(productId));
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> RemoveFromWishlist(int productId)
        {
            var result = await _mediator.Send(new RemoveFromWishlistCommand(productId));
            return StatusCode(result.StatusCode, result);
        }
    }
}

