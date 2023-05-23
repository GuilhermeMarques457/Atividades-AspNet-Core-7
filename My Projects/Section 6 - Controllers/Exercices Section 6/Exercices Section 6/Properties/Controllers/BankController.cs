using Exercices_Section_6.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exercices_Section_6.Properties.Controllers
{
    public class BankController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            Response.StatusCode = 200;
            return Content("Welcome to the Best Bank");
        }

        [Route("account-details")]
        public IActionResult Details()
        {
            Response.StatusCode = 200;
            var bank = new Bank()
            {
                accountNumber = 1001,
                accountHolderName = "Example Name",
                currentBalance = 500
            };
            return Json(bank);
        }

        [Route("account-statement")]
        public IActionResult pdf()
        {
            Response.StatusCode = 200;
            return new VirtualFileResult("Sample.pdf", "application/pdf");
        }

        [Route("get-current-balance/{accountNumber}")]
        public IActionResult GetCurrent()
        {
            int current = Convert.ToInt32(Request.RouteValues["accountNumber"]);
            if (current != 1001)
            {
                Response.StatusCode = 400;
                return Content("Account Number should be 1001");
            }

            Response.StatusCode = 200;
            return Content("5000");
        }


      
        //https://localhost:7152/account-current/312/true
        [Route("account-current/{currentBalance?}/{logged?}")]
        public IActionResult PrintCurrentRoute([FromRoute]int? currentBalance, bool? logged, [FromRoute]Bank bank)
        {
            if(currentBalance.HasValue == false)
            {
                return Content("There is no value sent");
            }
            if (logged.HasValue == false)
            {
                return Content("There logged value");
            }
            if (currentBalance < 0)
            {
                return Content("Negative value");
            }
            if (logged == false)
            {
                return StatusCode(401);
            }

            return Content(currentBalance + "");
        }

        //https://localhost:7152/account-current?current=10&lpgged=true
        [Route("account-current")]
        public IActionResult PrintCurrentQuery([FromQuery] int? current, [FromQuery] bool? logged)
        {
            if (current.HasValue == false)
            {
                return Content("There is no value sent");
            }
            if (logged.HasValue == false)
            {
                return Content("There logged value");
            }
            if (current < 0)
            {
                return Content("Negative value");
            }
            if (logged == false)
            {
                return StatusCode(401);
            }

            return Content(current + "");
        }
    }
}
