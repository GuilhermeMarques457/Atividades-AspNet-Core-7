using Microsoft.AspNetCore.Mvc;

namespace ViewsExample.Controllers
{
    public class ProductsController : Controller
    {
        [Route("products/all")]
        public IActionResult All()
        {
            return View();
            //Views/Products/All.cshtml
            //If this file above is not found the controller will search in the shared folder
            //Views/Shared/All.cshtml
        }
    }
}
