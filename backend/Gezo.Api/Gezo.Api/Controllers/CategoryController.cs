using Gezo.Application.Features.Categories.Queries.GetAllCategoryQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gezo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
       private readonly IMediator _mediator;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult> GetAllCategories()
        {

            return Ok(await _mediator.Send(new GetAllCategoriesQuery()));
        }
    }
}
