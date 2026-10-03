using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gezo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        []
        public async Task GetProducts()
        {
            return Ok();
        }
    }
}
