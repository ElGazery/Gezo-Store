using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Brands.Queries.GetAllBrandsQuery;
using MediatR;
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
        public async Task<ApiResponse<IReadOnlyList<GetAllBrandsQueryDto>>> GetAllBrands(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetAllBrandsQuery(),  cancellationToken);
        }
    }
}
