using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Addresses.Commands.AddAddressCommand;
using Gezo.Application.Features.Addresses.Commands.UpdateAddressCommand;
using Gezo.Application.Features.Addresses.Dtos;
using Gezo.Application.Features.Addresses.Queries.GetAddressQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gezo.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<AddressDto>>> GetAddress()
        {
            var result = await _mediator.Send(new GetAddressQuery());
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<AddressDto>>> AddAddress([FromBody] AddAddressCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<AddressDto>>> UpdateAddress([FromBody] UpdateAddressCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}
