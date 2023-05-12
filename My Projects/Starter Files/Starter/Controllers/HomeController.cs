using Microsoft.AspNetCore.Mvc;

namespace DepedencyInjectionExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

       
    }
}
