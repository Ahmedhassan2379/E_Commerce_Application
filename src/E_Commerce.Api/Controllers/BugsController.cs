using E_Commerce.Api.Errors;
using E_Commerce.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BugsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("not-found")]
        public ActionResult GetNotFound()
        {
            var product = _context.Products.Find(50);
            if (product is null)
            {
                return NotFound(new BaseCommonResponse(404));
            }
            return Ok(product);
        }
        [HttpGet("server-error")]
        public ActionResult GetServerError()
        {
            var product = _context.Products.Find(50);
            product.Name = "";
            return Ok();
        }
        [HttpGet("bad-request/{id}")]
        public ActionResult GetNotfoundRequest(int id)
        {
            return Ok();
        }
        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new BaseCommonResponse(400));
        }
    }
}
