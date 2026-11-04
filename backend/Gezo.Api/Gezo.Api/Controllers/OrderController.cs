using Gezo.Application.Features.Orders.Commands.AddOrderCommand;
using Gezo.Application.Features.Orders.Commands.UpdateOrderStatusCommand;
using Gezo.Application.Features.Orders.Queries.GetAllDeliveryMethods;
using Gezo.Application.Features.Orders.Queries.GetAllOrdersForAdminQuery;
using Gezo.Application.Features.Orders.Queries.GetOrderByIdQuery;
using Gezo.Application.Features.Orders.Queries.GetUserOrdersQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gezo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddOrder(AddOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetUserOrders()
        {
            var result = await _mediator.Send(new GetUserOrdersQuery());
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderById(int id)
        {
            var result = await _mediator.Send(new GetOrderByIdQuery(id));
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult> GetDeliveryMethods()
        {
            var result = await _mediator.Send(new GetAllDeliveryMethods());
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/GetAllOrders")]
        public async Task<ActionResult> GetAllOrdersAdmin([FromQuery] GetAllOrdersForAdminQuery query)
        {
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("admin/{id}/status")]
        public async Task<ActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}
