using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Linq;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService()
        {
            _countries = new List<Country>();
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            switch (countryAddRequest)
            {
                case null:
                    throw new ArgumentNullException(nameof(countryAddRequest));
            }

            switch (countryAddRequest.CountryName)
            {
                case null:
                    throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            if (_countries.Where(country => country.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("Given country name already exists");
            };

            Country country = countryAddRequest.ToCountry();
         
            country.CountyId = Guid.NewGuid(); 

            _countries.Add(country);

            return country.ToCountryResponse();

        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(c => c.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryById(Guid? countryID)
        {
            if (countryID == null) 
            {
                return null;
            }
            else
            {
                Country? country = _countries.FirstOrDefault(c => c.CountyId == countryID);

                if(country == null)
                {
                    return null;
                }

                return country.ToCountryResponse();
            }
            

            
        }
    }
}