using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Products.Commands.AddProductCommand;
using Gezo.Application.Features.Products.Commands.DeleteProductCommand;
using Gezo.Application.Features.Products.Commands.UpdateProductCommand;
using Gezo.Application.Features.Products.Queries.GetAllProductsQuery;
using Gezo.Application.Features.Products.Queries.GetProdcutByIdQuery;
using Gezo.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("GetProducts")]
        public async Task<ActionResult> GetProducts([FromQuery] GetAllProductsQuery Query)
        {
            
            return Ok(await _mediator.Send(Query));
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<ApiResponse<GetProductByIdQueryDto>>> GetProductById(int id)
        {

            return Ok(await _mediator.Send(new GetProdcutByIdQuery(id)));
        }
        [Authorize(Roles ="Admin")]
        [HttpPost("AddProduct")]
        public async Task<ActionResult> AddProduct([FromForm] AddProductCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Roles ="Admin")]
        [HttpPut]
        public async Task<ActionResult> UpdateProduct([FromForm] UpdateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteProduct")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));
            return StatusCode(result.StatusCode, result);
        }

    }
}
