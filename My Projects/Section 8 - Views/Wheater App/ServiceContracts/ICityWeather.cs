using Models;

namespace ServiceContracts
{
    public interface ICityWeather
    {
        List<CityWeather> GetWeatherDetails();
        CityWeather GetCityWeatherByCityCode(string cityCode);

    }
}