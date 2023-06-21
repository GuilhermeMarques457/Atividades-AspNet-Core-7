using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using AutoFixture;
using FluentAssertions;

namespace Tests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _peopleService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();

            var countriesInitialData = new List<Country>() { };
            var peopleInitialData = new List<Person>() { };

            DbContextMock<AspNetDbContext> dbContextMock = new DbContextMock<AspNetDbContext>(
                new DbContextOptionsBuilder<AspNetDbContext>().Options
            );
          
            var dbContext = dbContextMock.Object;

            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, peopleInitialData);

            _countriesService = new CountriesService(dbContext);
            _peopleService = new PersonService(dbContext, _countriesService);

            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson

        [Fact]
        public async Task AddPerson_NullPerson()
        {
            PersonAddRequest? addRequest = null;

            Func<Task> action = (async () =>
            {
               await _peopleService.AddPerson(addRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();

        }

        [Fact]
        public async Task AddPerson_NulNamePerson()
        {
            PersonAddRequest? addRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, null as string)
                .Create();

            Func<Task> action = (async () =>
            {
                await _peopleService.AddPerson(addRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddPerson_ProperPersonDetails()
        {
            //Create will just create the properties based on the type,
            //Validation like email doesn't matter
            //PersonAddRequest? addRequest = _fixture.Create<PersonAddRequest>();

            //Build override the default values with the method "with"
            PersonAddRequest? addRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();

            PersonResponse personResponseAdd = await _peopleService.AddPerson(addRequest);
            List<PersonResponse> personList = await _peopleService.GetPersonList();

            personResponseAdd.PersonId.Should().NotBeEmpty();
            personList.Should().Contain(personResponseAdd);

            
        }

        #endregion

        #region GetPersonById
        [Fact]
        public async Task GetPersonById_NullPersonId()
        {
            Guid? guid = null;

            PersonResponse? personResponse = await _peopleService.GetPersonById(guid);

            //Assert.Null(personResponse);
            personResponse.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonById_ValidIdPerson()
        {
            CountryAddRequest? countryAddRequest = _fixture.Build<CountryAddRequest>().Create();

            CountryResponse countryResponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();

            PersonResponse personResponse = await _peopleService.AddPerson(personAddRequest);

            PersonResponse? personResponseById = await _peopleService.GetPersonById(personResponse.PersonId);

            personResponseById.Should().Be(personResponseById);

            //Assert.Equal(personResponse, personResponseById);
        }
        #endregion

        #region GetAllPeople

        [Fact]
        public async Task GetPersonList_EmptyList()
        {
            List<PersonResponse> personResponses = await _peopleService.GetPersonList();

            personResponses.Should().BeEmpty();
        }

        [Fact]
        public async Task GetPersonList_ProperList()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Build<CountryAddRequest>().Create();
            CountryAddRequest country_request_2 = _fixture.Build<CountryAddRequest>().Create();

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _peopleService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            _testOutputHelper.WriteLine("Expected:");
            //print person_response_list
            foreach (PersonResponse person_response_list in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_get = await _peopleService.GetPersonList();

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_list in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //Assert
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    persons_list_from_get.Should().Contain(person_response_from_add);
            //}

            //Same thing as above
            persons_list_from_get.Should().BeEquivalentTo(persons_list_from_get);

        }
        #endregion

        #region GetFilteredPeople
        [Fact]
        public async Task GetFilteredPeople_EmptySearch()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Build<CountryAddRequest>().Create();
            CountryAddRequest country_request_2 = _fixture.Build<CountryAddRequest>().Create();

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create(); 
            

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _peopleService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            _testOutputHelper.WriteLine("Expected:");
            //print person_response_list
            foreach (PersonResponse person_response_list in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //Act
            List<PersonResponse>? persons_list_from_search = await _peopleService.GetFilterdPeople(nameof(Person.PersonName), "");

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_list in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //Assert
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    persons_list_from_search.Should().Contain(person_response_from_add);
            //}

            persons_list_from_search.Should().BeEquivalentTo(person_response_list_from_add);
        }

        #endregion

        #region GetFilteredPersonByName

     
        //First we add people and then we search a string
        //And then it should return based on that string
        [Fact]
        public async Task GetFilteredPeople_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Build<CountryAddRequest>().Create();
            CountryAddRequest country_request_2 = _fixture.Build<CountryAddRequest>().Create();

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.PersonName, "Marta")
                .Create();

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _peopleService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            _testOutputHelper.WriteLine("Expected:");
            //print person_response_list
            foreach (PersonResponse person_response_list in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //Act
            List<PersonResponse>? persons_list_from_search = await _peopleService.GetFilterdPeople(nameof(Person.PersonName), "ma");

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_list in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //Assert
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    if(person_response_from_add.PersonName != null)
            //    {
            //        if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
            //        {
            //            persons_list_from_search.Should().Contain(person_response_from_add);
            //        }
            //    } 
            //}

            persons_list_from_search.Should().OnlyContain(temp => temp.PersonName!.Contains("ma", StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region GetSortedPeople
        [Fact]
        public async Task GetSortedPeople()
        {
            
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Build<CountryAddRequest>().Create();
            CountryAddRequest country_request_2 = _fixture.Build<CountryAddRequest>().Create();

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();
            

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();

        List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _peopleService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            _testOutputHelper.WriteLine("Expected:");
            

            List<PersonResponse> person_list = await _peopleService.GetPersonList();
            //Act
            List<PersonResponse>? persons_list_from_sort = _peopleService
                .GetSortedPeople(person_list, nameof(Person.PersonName), SortOrderOptions.DESC);

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_list in persons_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //person_response_list_from_add =
                //person_response_list_from_add.OrderByDescending(temp => temp.PersonName).ToList();

            //print person_response_list
            foreach (PersonResponse person_response_list in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //Assert
            //for (int i = 0; i <person_response_list_from_add.Count; i++)
            //{
            //    person_response_list_from_add[i].Should().Be(persons_list_from_sort[i]);
            //}
            persons_list_from_sort.Should().BeInDescendingOrder(temp => temp.PersonName);
        }
        #endregion

        #region UpdatePerson

        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            PersonUpdateRequest? personUpdateRequest = null;

             Func<Task> action = (async() =>
             {
                 await _peopleService.UpdatePerson(personUpdateRequest);
             });

             await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdatePerson_InvalidPersonId()
        {
            PersonUpdateRequest? personUpdateRequest = _fixture.Build<PersonUpdateRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.PersonId, Guid.NewGuid)
                .Create();


            Func<Task> action = (async() =>
             {
                 await _peopleService.UpdatePerson(personUpdateRequest);
             });

             await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdatePerson_PersonNameNull()
        {
            CountryAddRequest country = _fixture.Build<CountryAddRequest>().Create();

            CountryResponse countryResponse = await _countriesService.AddCountry(country);

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();

            PersonResponse personResponse = await _peopleService.AddPerson(personAddRequest);

            PersonUpdateRequest? personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            Func<Task> action = (async () =>
            {
                await _peopleService.UpdatePerson(personUpdateRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();

        }

        [Fact]
        public async Task UpdatePerson_ProperDetails()
        {
            CountryAddRequest country = _fixture.Build<CountryAddRequest>().Create();

            CountryResponse countryResponse = await _countriesService.AddCountry(country);

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();

            PersonResponse personResponse = await _peopleService.AddPerson(personAddRequest);

            PersonUpdateRequest? personUpdateRequest = personResponse.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = "Lindo";
            personUpdateRequest.PersonEmail = "lindo@gmail.com";

            PersonResponse personResponseFromUpdate = await _peopleService.UpdatePerson(personUpdateRequest);

            PersonResponse? personResponseFromGet = await _peopleService.GetPersonById(personResponseFromUpdate.PersonId);

            personResponseFromGet.Should().Be(personResponseFromUpdate);

        }

        #endregion

        #region DeletePerson
  
        [Fact]
        public async Task DeletePerson_ValidPersonID()
        {
            CountryAddRequest country_request_1 = _fixture.Build<CountryAddRequest>().Create();

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();

            PersonResponse person_response = await _peopleService.AddPerson(person_request_1);

            bool isDeleted = await _peopleService.DeletePerson(person_response.PersonId);

            isDeleted.Should().BeTrue();

            //Assert.True(isDeleted);
        }

        //If supplied id doesn't existis, it returns false
        [Fact]
        public async Task DeletePerson_inValidPersonID()
        {
            bool isDeleted = await _peopleService.DeletePerson(Guid.NewGuid());

            isDeleted.Should().BeFalse();
            //Assert.False(isDeleted);
        }

        #endregion
    }
}
