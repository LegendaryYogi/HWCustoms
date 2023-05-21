using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers               //49
{
    public class BuggyController : BaseApiController
    {
        public readonly StoreContext _context;
        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var thing = _context.Products.Find(42);    //find if not entity find return null

            if (thing == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok();
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var thing = _context.Products.Find(42);

            var thingToReturn = thing.ToString();   //it generates an exeption, it cannot execute tostring method on a string that doesn't exist (null)
            
            return Ok();
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));        //400 bad request
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetNotFoundRequest(int id)      //passing a string instead of int generates error
        {
            return Ok();
        }
    }
}