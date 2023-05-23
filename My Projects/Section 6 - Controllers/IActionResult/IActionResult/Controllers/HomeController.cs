using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace IActionResult.Controllers
{
    public class HomeController : Controller
    {
        [Route("bookstore")]
        public ActionResult Index(int bookid)
        {
            //Book id is not supplied
            if (!Request.Query.ContainsKey("bookid"))
            {
                //Response.StatusCode = 400;
                //return Content("Book id is not supplied");
                return BadRequest("Book id is not supplied");
            }

            //Book id can't be empty
            if(string.IsNullOrEmpty(Convert.ToString
                (Request.Query["bookid"])))
            {

                //return Content("Book id can't be null");
                return BadRequest("Book id can't be null");
            }

            //Book id should be between 1 to 1000
            int bookId = Convert.ToInt16(ControllerContext.HttpContext.Request.Query["bookid"]);
            if (bookId <= 0)
            {
                //Response.StatusCode = 400;
                //return Content("Book id cannot be less than zero");
                return BadRequest("Book id cannot be less than zero");
            }
            if (bookId > 1000)
            {
                //Response.StatusCode = 404;
                //return Content("Book id cannot be greater than 1000");
                return NotFound("Book id cannot be greater than 1000");
            }

            //isloggedin should be true
            if (Convert.ToBoolean(Request.Query["isloogedin"]) == false)
            {
                //Response.StatusCode = 401; 
                //return Content("User must be logged in");
                return Unauthorized("User must be logged in");

            }

            //302 - Found
            //return new RedirectToActionResult("Books", "Store", new { }); 
            //return RedirectToAction("Books", "Store", new { id = bookId });
            //return new RedirectResult($"store/books/{bookId}");

            //302 - Found - Local
            //return new LocalRedirectResult($"store/books/{bookId}");
            //return LocalRedirect($"store/books/{bookId}");

            //301 - Permanently
            //return new RedirectToActionResult("Books", "Store", new { }, permanent: true); // 301 - moved permanently
            //return RedirectToActionPermanent("Books", "Store", new { id = bookId })
            //return LocalRedirectPermanent($"store/books/{bookId}");
            //return RedirectPermanent($"store/books/{bookId}");

            return NotFound();








        }
    }
}
