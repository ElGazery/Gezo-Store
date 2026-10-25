using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Accounts.Commands.LoginCommand;
using Gezo.Application.Features.Accounts.Commands.LogoutCommand;
using Gezo.Application.Features.Accounts.Commands.RefreshTokenCommand;
using Gezo.Application.Features.Accounts.Commands.RegisterCommand;
using Gezo.Application.Features.Accounts.Queries.GetCurrentUserQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpGet("Me")]
        public async Task<ActionResult> Me()
        {
            var result = await _mediator.Send(new GetCurrentUserQuery());
            return StatusCode(result.StatusCode, result);
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
        [Authorize]
        [HttpPost("RefreshToken")]
        public async Task<ActionResult> RefreshToken()
        {
            var result = await _mediator.Send(new RefreshTokenCommand());
            return StatusCode(result.StatusCode, result);
        }
        [AllowAnonymous]
        [HttpDelete("Logout")]
        public async Task<ActionResult> Logout()
        {
            var result = await _mediator.Send(new LogoutCommand());
            return StatusCode(result.StatusCode,result);
        }

    }
}
