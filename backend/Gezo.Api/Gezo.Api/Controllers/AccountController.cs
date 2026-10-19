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
        //[HttpPost("Register")]
        //public Task<ActionResult> Register()
        //{
        //    return Ok();
        //}
        //[HttpPost("Login")]
        //public Task<AcceptedResult> Login()
        //{
        //    return Ok();
        //}

    }
}
