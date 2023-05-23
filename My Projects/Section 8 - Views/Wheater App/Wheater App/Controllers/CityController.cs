using Microsoft.AspNetCore.Mvc;
using Models;
using ServiceContracts;


namespace Wheater_App.Controllers
{
    public class CityController : Controller
    {
        private readonly ICityWeather _cityWeatherObj;

        public CityController(ICityWeather cityWeatherObj)
        {
            _cityWeatherObj = cityWeatherObj;
        }

        //IList<CityWeather> weatherList = new List<CityWeather>();
        //CityWeather cityWeather1 = new CityWeather()
        //{
        //    CityUniqueCode = "LDN",
        //    CityName = "London",
        //    DateAndTime = "2030-01-01 8:00",
        //    TemperatureFahrenheit = 33
        //};
        //CityWeather cityWeather2 = new CityWeather()
        //{
        //    CityUniqueCode = "NYC",
        //    CityName = "London",
        //    DateAndTime = "2030-01-01 3:00",
        //    TemperatureFahrenheit = 60
        //};
        //CityWeather cityWeather3 = new CityWeather()
        //{
        //    CityUniqueCode = "PAR",
        //    CityName = "Paris",
        //    DateAndTime = "2030-01-01 9:00",
        //    TemperatureFahrenheit = 82
        //};

        [Route("/")]
        public IActionResult Index()
        {
            ViewBag.Title = "WeatherCity";

            //weatherList.Add(cityWeather1);
            //weatherList.Add(cityWeather2);
            //weatherList.Add(cityWeather3);

            var cities = _cityWeatherObj.GetWeatherDetails();

            return View(cities);
        }

        [Route("Weather/{CityUnicCode}")]
        public IActionResult Details(string? CityUnicCode)
        {
            ViewBag.Title = "DetailsWeatherCity";

            if (CityUnicCode == null)
            {
                return NotFound();
            }
            else
            {
                CityWeather? city = _cityWeatherObj.GetCityWeatherByCityCode(CityUnicCode);

                return View(city);
            }

         

           
            

       
        }
    }
}
