using ConfigurationExample;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DepedencyInjectionExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly WeatherApiOptions _options;

        public HomeController(IOptions<WeatherApiOptions> weatherApiOptions /*IConfiguration configuration*/)
        {
            _options = weatherApiOptions.Value;
            //_configuration = configuration
        }

        [Route("/")]
        public IActionResult Index()
        {
            //Get loads configurations into a new options object
            //WeatherApiOptions options = 
            //    _configuration.GetSection("weatherApi")
            //    .Get<WeatherApiOptions>();

            //Bind load configurations into an existing options object
            //WeatherApiOptions optionsBind = new WeatherApiOptions();
            //_configuration.GetSection("weatherApi").Bind(optionsBind);

            ViewBag.ClientID = _options.ClientID;
            ViewBag.Secret = _options.ClientSecret;
            //ViewBag.MyKey = _configuration.GetValue<string>("MyKey");
            //ViewBag.ClientID = options.ClientID;
            //ViewBag.Secret = optionsBind.ClientSecret;
            //ViewBag.Secret = _configuration.GetValue<string>("weatherApi:ClientSecret");
            //ViewBag.ClientID = _configuration.GetSection("weatherApi")["ClientID"];
            return View();
            
        }

       
    }
}
