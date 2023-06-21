using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using EntityFrameworkCoreMock;
using Moq;
using AutoFixture;

namespace Tests
{
    //I will let this tests with default assertions, not with fluent assertions

    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly IFixture _fixture;

        public CountriesServiceTest()
        {
            _fixture = new Fixture();

            var countriesInitialData = new List<Country>() { };

            //Mocking the dbContext
            DbContextMock<AspNetDbContext> dbContextMock = new DbContextMock<AspNetDbContext>(
                new DbContextOptionsBuilder<AspNetDbContext>().Options
            );

            //Taking an object of the dbcontext through the mock object
            var dbContext = dbContextMock.Object;

            //Creating a dummy instance of the dbSet country entity
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);

            _countriesService = new CountriesService(dbContext); 
        }

        #region AddCountry
        //When CountryAddRequest is null, it should throw ArgumentNullExeption
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request);
            });
            
        }

        //When CountryName is null, it should throw ArgumentNullExeption
        [Fact]
        public async Task AddCountry_NullNameCountry()
        {
            //Arrange
            CountryAddRequest? request = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, null as string)
                .Create();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request);
            });
        }

        //When the CountryName is duplicate, it should throw ArgumentExeption
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? request1 = _fixture.Build<CountryAddRequest>().Create();

            CountryAddRequest? request2 = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, request1.CountryName)
                .Create();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);
            });
        }

        //When supplied country is correct, it should insert the country to the existing list of countries
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest? request = _fixture.Build<CountryAddRequest>().Create();

            //Act
            CountryResponse response = await _countriesService.AddCountry(request);
            List<CountryResponse> allCountries = await _countriesService.GetAllCountries();

            //Assert
            Assert.True(response.CountryId != Guid.Empty);
            //It loops to allCountries trying to find the element, like counties of js
            Assert.Contains(response, allCountries);
        }
        #endregion

        #region GetAllCountries

        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            //Act
            List<CountryResponse> actueal_country_response_list =
                await _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(actueal_country_response_list);
        }

        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            //Act

            CountryAddRequest countryAddRequest1 = _fixture.Build<CountryAddRequest>().Create();
            CountryAddRequest countryAddRequest2 = _fixture.Build<CountryAddRequest>().Create();
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
            {
                countryAddRequest1,
                countryAddRequest2
            };

            //Act
            List<CountryResponse> countriesListFromAddCountry = new List<CountryResponse>();

            foreach(CountryAddRequest countryRequest in country_request_list)
            {
                countriesListFromAddCountry.Add(await _countriesService.AddCountry(countryRequest));
                    
            }

            List<CountryResponse> actualCountryResponseList = await _countriesService.GetAllCountries();

            foreach (CountryResponse expectedCountry in countriesListFromAddCountry)
            {
                Assert.Contains(expectedCountry, actualCountryResponseList);
            }

            //Assert
           
        }

        #endregion

        #region GetCountryByID

        [Fact]
        public async Task GetCountryById_NullCountryID()
        {
            //Arrange
            Guid? countryId = null;

            //Act
            CountryResponse? countryResponse = await _countriesService.GetCountryById(countryId);

            //Assert
            Assert.Null(countryResponse);
        }

        [Fact]
        public async Task GetCountryById_ValidCountryId()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = _fixture.Build<CountryAddRequest>().Create();

            CountryResponse countryResponseAdd = await _countriesService.AddCountry(countryAddRequest);

            //Act
            CountryResponse? countryResponseByGetId = await _countriesService.GetCountryById(countryResponseAdd.CountryId);

            //Assert
            Assert.Equal(countryResponseAdd, countryResponseByGetId);
        }

        #endregion
    }
}
