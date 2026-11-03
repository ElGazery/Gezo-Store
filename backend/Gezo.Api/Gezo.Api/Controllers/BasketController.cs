using Gezo.Application.Features.Basket.Commands.DeleteBasketCommand;
using Gezo.Application.Features.Basket.Commands.DeleteItemCommand;
using Gezo.Application.Features.Basket.Commands.UpdateBasketCommand;
using Gezo.Application.Features.Basket.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities;

namespace Gezo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BasketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Basket( )
        {
            var result = await _mediator.Send(new GetBasketQuery());
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateBasket(UpdateBasketCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete("deleteItem")]
        public async Task<ActionResult> DeleteItem(DeleteItemCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteBasket()
        {
            var result = await _mediator.Send(new DeleteBasketCommand());
            return StatusCode(result.StatusCode, result);

        }
    }
}
