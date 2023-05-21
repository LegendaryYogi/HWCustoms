using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("errors/{code}")]  //code = status code or the error     51
    [ApiExplorerSettings(IgnoreApi = true)]  //it tells swagger to ignore this error controller 54

    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}