using Gezo.Application.Features.Payments.Commands.CreatePaymentIntentCommand;
using Gezo.Application.Features.Payments.Commands.StripeWebhookCommand;
using Gezo.Application.Features.Payments.Queries.GetPaymentStatusQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Gezo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("{orderId}")]
        public async Task<ActionResult> CreatePaymentIntent(int orderId)
        {
            var result = await _mediator.Send(new CreatePaymentIntentCommand(orderId));
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<ActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"].ToString();
            var result = await _mediator.Send(new StripeWebhookCommand(json, signature));
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("{orderId}/status")]
        public async Task<ActionResult> GetPaymentStatus(int orderId)
        {
            var result = await _mediator.Send(new GetPaymentStatusQuery(orderId));
            return StatusCode(result.StatusCode, result);
        }
    }
}
