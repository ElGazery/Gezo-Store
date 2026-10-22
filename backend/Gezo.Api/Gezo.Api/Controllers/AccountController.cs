using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Accounts.Commands.LoginCommand;
using Gezo.Application.Features.Accounts.Commands.RegisterCommand;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gezo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
            
        }
        [HttpPost("Register")]
        public async Task<ActionResult<ApiResponse<RegisterResponseDto>>> Register([FromBody]RegisterCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command,cancellationToken);
            return StatusCode(result.StatusCode,result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return StatusCode(result.StatusCode,result);
        }


    }
}
