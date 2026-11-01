using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Brands.Commands.AddBrandCommand;
using Gezo.Application.Features.Brands.Commands.DeleteBrandCommand;
using Gezo.Application.Features.Brands.Commands.UpdateBrandCommand;
using Gezo.Application.Features.Brands.Queries.GetAllBrandsQuery;
using Gezo.Application.Features.Brands.Queries.GetBrandByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gezo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllBrands")]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllBrandsQueryDto>>>> GetAllBrands(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetAllBrandsQuery(), cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBrandById(int id)
        {
            var result = await _mediator.Send(new GetBrandByIdQuery(id));
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddBrand([FromForm] AddBrandCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBrand(int id, [FromForm] UpdateBrandCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            var result = await _mediator.Send(new DeleteBrandCommand(id));
            return StatusCode(result.StatusCode, result);
        }
    }
}
