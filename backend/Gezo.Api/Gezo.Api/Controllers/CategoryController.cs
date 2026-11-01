using Gezo.Application.Features.Categories.Commands.AddCategoryCommand;
using Gezo.Application.Features.Categories.Commands.DeleteCategoryCommand;
using Gezo.Application.Features.Categories.Commands.UpdateCategoryCommand;
using Gezo.Application.Features.Categories.Queries.GetAllCategoryQuery;
using Gezo.Application.Features.Categories.Queries.GetCategoryByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;

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
        [Authorize]
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult> GetAllCategories()
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery());
            return StatusCode(result.StatusCode,result);
        }
        [Authorize]
        [HttpGet("GetCategoryById")]
        public async Task<ActionResult> GetCategoryById(int id)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id));
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Roles ="Admin")]
        [HttpPost("AddCategory")]
        public async Task<ActionResult> AddCategory(AddCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult> UpdateCategory(UpdateCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Roles ="Admin")]
        [HttpDelete]
        public async Task<ActionResult> DeleteCategory(int id )
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(id));
            return StatusCode(result.StatusCode, result);
        }
    }
}
