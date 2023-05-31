using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Linq;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();

            if(initialize)
            {
                _countries.AddRange(new List<Country>() {
                    new Country() { CountyId = Guid.Parse("000C76EB-62E9-4465-96D1-2C41FDB64C3B"), CountryName = "USA" },

                    new Country() { CountyId = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F"), CountryName = "Canada" },

                    new Country() { CountyId = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E"), CountryName = "UK" },

                    new Country() { CountyId = Guid.Parse("15889048-AF93-412C-B8F3-22103E943A6D"), CountryName = "India" },

                    new Country() { CountyId = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB"), CountryName = "Australia" }
                });
            }
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