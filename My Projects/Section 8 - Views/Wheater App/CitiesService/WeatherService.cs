using Models;
using ServiceContracts;

namespace CitiesService
{
    public class WeatherService : ICityWeather
    {

        private readonly List<CityWeather>? _cities;

        public WeatherService()
        {

            _cities = new List<CityWeather>()
            {
                new CityWeather()
                {
                    CityUniqueCode = "LDN",
                    CityName = "London",
                    DateAndTime = "2030-01-01 8:00",
                    TemperatureFahrenheit = 33
                },
                new CityWeather()
                {
                    CityUniqueCode = "LDN",
                    CityName = "London",
                    DateAndTime = "2030-01-01 8:00",
                    TemperatureFahrenheit = 33
                },
                new CityWeather()
                {
                    CityUniqueCode = "PAR",
                    CityName = "Paris",
                    DateAndTime = "2030-01-01 9:00",
                    TemperatureFahrenheit = 82
                }
            };
        }

      

        public CityWeather GetCityWeatherByCityCode(string cityCode)
        {
            CityWeather? city = _cities.FirstOrDefault(x => x.CityUniqueCode == cityCode);
            return city;
        }

        public List<CityWeather> GetWeatherDetails()
        {
            return _cities;
        }
    }
}