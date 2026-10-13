using Gezo.Application.Features.Products.Queries.GetProdcutByIdQuery;
using Gezo.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gezo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //[HttpGet("GetProducts")]
        ////public async Task<ICollection<>> GetProducts()
        //{
        //    return Ok();
        //}
        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<GetProdcutByIdQuery>> GetProductById(int id)
        {

            return Ok(await _mediator.Send(new GetProdcutByIdQuery(id)));
        }

    }
}
