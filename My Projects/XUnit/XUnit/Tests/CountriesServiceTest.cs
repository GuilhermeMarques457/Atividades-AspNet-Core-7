using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(false);
        }

        #region AddCountry
        //When CountryAddRequest is null, it should throw ArgumentNullExeption
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
            
        }

        //When CountryName is null, it should throw ArgumentNullExeption
        [Fact]
        public void AddCountry_NullNameCountry()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }

        //When the CountryName is duplicate, it should throw ArgumentExeption
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        //When supplied country is correct, it should insert the country to the existing list of countries
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Japan"
            };                     

            //Act
            CountryResponse response = _countriesService.AddCountry(request);
            List<CountryResponse> allCountries = _countriesService.GetAllCountries();

            //Assert
            Assert.True(response.CountryId != Guid.Empty);
            //It loops to allCountries trying to find the element, like counties of js
            Assert.Contains(response, allCountries);
        }
        #endregion

        #region GetAllCountries

        [Fact]
        public void GetAllCountries_EmptyList()
        {
            //Act
            List<CountryResponse> actueal_country_response_list =
                _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(actueal_country_response_list);
        }

        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            //Act
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
            {
                new CountryAddRequest() { CountryName= "Usa"},
                new CountryAddRequest() { CountryName= "Brasil"}
            };

            //Act
            List<CountryResponse> countriesListFromAddCountry = new List<CountryResponse>();

            foreach(CountryAddRequest countryRequest in country_request_list)
            {
                countriesListFromAddCountry.Add(_countriesService.AddCountry(countryRequest));
                    
            }

            List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

            foreach (CountryResponse expectedCountry in countriesListFromAddCountry)
            {
                Assert.Contains(expectedCountry, actualCountryResponseList);
            }

            //Assert
           
        }

        #endregion

        #region GetCountryByID

        [Fact]
        public void GetCountryById_NullCountryID()
        {
            //Arrange
            Guid? countryId = null;

            //Act
            CountryResponse? countryResponse = _countriesService.GetCountryById(countryId);

            //Assert
            Assert.Null(countryResponse);
        }

        [Fact]
        public void GetCountryById_ValidCountryId()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() 
            { 
                CountryName = "Brasil"
            };
            CountryResponse countryResponseAdd = _countriesService.AddCountry(countryAddRequest);

            //Act
            CountryResponse? countryResponseByGetId = _countriesService.GetCountryById(countryResponseAdd.CountryId);

            //Assert
            Assert.Equal(countryResponseAdd, countryResponseByGetId);
        }

        #endregion
    }
}
