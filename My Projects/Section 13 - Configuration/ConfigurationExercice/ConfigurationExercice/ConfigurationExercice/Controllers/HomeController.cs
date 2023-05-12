using ConfigurationExercice.Options;
using Microsoft.AspNetCore.Mvc;

namespace DepedencyInjectionExample.Controllers
{
    public class HomeController : Controller
    {

        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("/")]
        public IActionResult Index()
        {
            SocialMediaLinksOptions options = _configuration.GetSection("SocialMediaLinks").Get<SocialMediaLinksOptions>();
            //ViewBag.Facebook = _configuration.GetValue<string>("Facebook");
            //ViewBag.Instagram = _configuration.GetValue<string>("Instagram");
            //ViewBag.Twitter = _configuration.GetValue<string>("Twitter");
            //ViewBag.Youtube = _configuration.GetValue<string>("Youtube");
            ViewBag.Facebook = options.Facebook;
            ViewBag.Twitter = options.Twitter;
            ViewBag.Instagram = options.Instagram;
            ViewBag.Youtube = options.Youtube;

            return View();
        }

       
    }
}
