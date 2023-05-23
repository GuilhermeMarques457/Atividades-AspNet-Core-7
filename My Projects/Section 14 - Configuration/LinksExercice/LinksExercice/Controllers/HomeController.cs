using LinksExercice.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DepedencyInjectionExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SocialMediaLinksOptions _options;
        public HomeController(IConfiguration configuration, IOptions<SocialMediaLinksOptions> options)
        {
            _options = options.Value;
            _configuration = configuration;
        }

        [Route("/")]
        public IActionResult Index()
        {
            //SocialMediaLinksOptions options = _configuration.GetSection("SocialMediaLinks").Get<SocialMediaLinksOptions>();
            //ViewBag.Facebook = _configuration.GetValue<string>("Facebook");
            //ViewBag.Instagram = _configuration.GetValue<string>("Instagram");
            //ViewBag.Twitter = _configuration.GetValue<string>("Twitter");
            //ViewBag.Youtube = _configuration.GetValue<string>("Youtube");
            ViewBag.Facebook = _options.Facebook;
            ViewBag.Twitter = _options.Twitter;
            ViewBag.Instagram = _options.Instagram;
            ViewBag.Youtube = _options.Youtube;

            return View();
        }

    }
}
