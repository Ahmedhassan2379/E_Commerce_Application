using E_Commerce.Api.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Api.Controllers
{
    [Route("errors/{statusCode}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi =true)]
    public class ErrorController : ControllerBase
    {
        //[HttpGet("Error")]
        public IActionResult Error(int statusCode)
        {
            return new ObjectResult(new BaseCommonResponse(statusCode));
        }
    }
}
