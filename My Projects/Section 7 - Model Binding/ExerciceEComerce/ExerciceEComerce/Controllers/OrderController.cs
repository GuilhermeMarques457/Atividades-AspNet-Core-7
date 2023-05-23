using ExerciceEComerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExerciceEComerce.Controllers
{
    public class OrderController : Controller
    {
        [Route("order")]
        public IActionResult Index(Order order)
        {
            if (ModelState.IsValid)
            {
                Random random = new Random();
                order.Id = random.Next(9999);

                return Content(order.Id.ToString());
            }
            else
            {
                List<string> errorsList = new List<string>();
                foreach (var values in ModelState.Values)
                {
                    foreach (var error in values.Errors)
                    {
                        errorsList.Add(error.ErrorMessage);

                    }
                }
                string errors = string.Join("\n", errorsList);
                return BadRequest(errors);
            }
       
        }
    }
}
