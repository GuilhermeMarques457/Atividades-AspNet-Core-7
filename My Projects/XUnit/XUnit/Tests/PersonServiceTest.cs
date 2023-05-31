using Entities;
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

namespace Tests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _peopleService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _peopleService = new PersonService(false);
            _countriesService = new CountriesService(false);
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson

        [Fact]
        public void AddPerson_NullPerson()
        {
            PersonAddRequest? addRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _peopleService.AddPerson(addRequest);
            });
        }

        [Fact]
        public void AddPerson_NulNamePerson()
        {
            PersonAddRequest? addRequest = new PersonAddRequest()
            {
                PersonName = null
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _peopleService.AddPerson(addRequest);
            });
        }

        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            PersonAddRequest? addRequest = new PersonAddRequest()
            {
                PersonName = "Guilherme",
                PersonAddress = "Antonio Buzzeto",
                CountryID = Guid.NewGuid(),
                PersonEmail = "guimarkes457@gmail.com",
                PersonGender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                DateOfBirth = DateTime.Parse("2006-03-12"),
            };

            PersonResponse personResponseAdd = _peopleService.AddPerson(addRequest);
            List<PersonResponse> personList = _peopleService.GetPersonList();

            Assert.True(personResponseAdd.PersonId != Guid.Empty);
            Assert.Contains(personResponseAdd, personList);
        }

        #endregion

        #region GetPersonById
        [Fact]
        public void GetPersonById_NullPersonId()
        {
            Guid? guid = null;

            PersonResponse? personResponse = _peopleService.GetPersonById(guid);

            Assert.Null(personResponse);
        }

        [Fact]
        public void GetPersonById_ValidIdPerson()
        {
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Brasil"
            };

            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonAddress = "Antonio Buzzeto",
                CountryID = countryResponse.CountryId,
                PersonEmail = "guimarkes457@gmail.com",
                PersonGender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                DateOfBirth = DateTime.Parse("2006-03-12"),
                PersonName = "Guilherme Marques dos Santos"
            };

            PersonResponse personResponse = _peopleService.AddPerson(personAddRequest);

            PersonResponse? personResponseById = _peopleService.GetPersonById(personResponse.PersonId);

            Assert.Equal(personResponse, personResponseById);
        }
        #endregion

        #region GetAllPeople

        [Fact]
        public void GetPersonList_EmptyList()
        {
            List<PersonResponse> personResponses = _peopleService.GetPersonList();

            Assert.Empty(personResponses);
        }

        [Fact]
        public void GetPersonList_ProperList()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                PersonEmail = "smith@example.com",
                PersonGender = GenderOptions.Male,
                PersonAddress = "address of smith",
                CountryID = country_response_1.CountryId,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                PersonEmail = "mary@example.com",
                PersonGender = GenderOptions.Female,
                PersonAddress = "address of mary",
                CountryID = country_response_2.CountryId,
                DateOfBirth = DateTime.Parse("2000-02-02"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Rahman",
                PersonEmail = "rahman@example.com",
                PersonGender = GenderOptions.Male,
                PersonAddress = "address of rahman",
                CountryID = country_response_2.CountryId,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _peopleService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            _testOutputHelper.WriteLine("Expected:");
            //print person_response_list
            foreach (PersonResponse person_response_list in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_get = _peopleService.GetPersonList();

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_list in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, persons_list_from_get);
            }

        }
        #endregion

        #region GetFilteredPeople
        [Fact]
        public void GetFilteredPeople_EmptySearch()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                PersonEmail = "smith@example.com",
                PersonGender = GenderOptions.Male,
                PersonAddress = "address of smith",
                CountryID = country_response_1.CountryId,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                PersonEmail = "mary@example.com",
                PersonGender = GenderOptions.Female,
                PersonAddress = "address of mary",
                CountryID = country_response_2.CountryId,
                DateOfBirth = DateTime.Parse("2000-02-02"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Rahman",
                PersonEmail = "rahman@example.com",
                PersonGender = GenderOptions.Male,
                PersonAddress = "address of rahman",
                CountryID = country_response_2.CountryId,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _peopleService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            _testOutputHelper.WriteLine("Expected:");
            //print person_response_list
            foreach (PersonResponse person_response_list in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //Act
            List<PersonResponse>? persons_list_from_search = _peopleService.GetFilterdPeople(nameof(Person.PersonName), "");

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_list in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, persons_list_from_search);
            }
        }

        #endregion

        #region GetFilteredPersonByName

     
        //First we add people and then we search a string
        //And then it should return based on that string
        [Fact]
        public void GetFilteredPeople_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                PersonEmail = "smith@example.com",
                PersonGender = GenderOptions.Male,
                PersonAddress = "address of smith",
                CountryID = country_response_1.CountryId,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                PersonEmail = "mary@example.com",
                PersonGender = GenderOptions.Female,
                PersonAddress = "address of mary",
                CountryID = country_response_2.CountryId,
                DateOfBirth = DateTime.Parse("2000-02-02"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Rahman",
                PersonEmail = "rahman@example.com",
                PersonGender = GenderOptions.Male,
                PersonAddress = "address of rahman",
                CountryID = country_response_2.CountryId,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _peopleService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            _testOutputHelper.WriteLine("Expected:");
            //print person_response_list
            foreach (PersonResponse person_response_list in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //Act
            List<PersonResponse>? persons_list_from_search = _peopleService.GetFilterdPeople(nameof(Person.PersonName), "ma");

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_list in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                if(person_response_from_add.PersonName != null)
                {
                    if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person_response_from_add, persons_list_from_search);
                    }
                }
               
                
            }
        }
        #endregion

        #region GetSortedPeople
        [Fact]
        public void GetSortedPeople()
        {
            
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                PersonEmail = "smith@example.com",
                PersonGender = GenderOptions.Male,
                PersonAddress = "address of smith",
                CountryID = country_response_1.CountryId,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                PersonEmail = "mary@example.com",
                PersonGender = GenderOptions.Female,
                PersonAddress = "address of mary",
                CountryID = country_response_2.CountryId,
                DateOfBirth = DateTime.Parse("2000-02-02"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Rahman",
                PersonEmail = "rahman@example.com",
                PersonGender = GenderOptions.Male,
                PersonAddress = "address of rahman",
                CountryID = country_response_2.CountryId,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _peopleService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            _testOutputHelper.WriteLine("Expected:");
            

            List<PersonResponse> person_list = _peopleService.GetPersonList();
            //Act
            List<PersonResponse>? persons_list_from_sort = _peopleService
                .GetSortedPeople(person_list, nameof(Person.PersonName), SortOrderOptions.DESC);

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_list in persons_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            person_response_list_from_add =
                person_response_list_from_add.OrderByDescending(temp => temp.PersonName).ToList();

            //print person_response_list
            foreach (PersonResponse person_response_list in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //Assert
            for (int i = 0; i <person_response_list_from_add.Count; i++)
            {
                Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
            }
        }
        #endregion

        #region UpdatePerson

        [Fact]
        public void UpdatePerson_NullPerson()
        {
            PersonUpdateRequest? personUpdateRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _peopleService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_InvalidPersonId()
        {
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest() 
            { 
                PersonId = Guid.NewGuid(),
            };
            

            Assert.Throws<ArgumentException>(() =>
            {
                _peopleService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_PersonNameNull()
        {
            CountryAddRequest country = new CountryAddRequest()
            {
                CountryName = "Brasil"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(country);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                CountryID = countryResponse.CountryId,
                PersonName = "Guilherme",
                PersonEmail = "guimarkes457@gmail.com",
                PersonAddress = "address kakakak",
                PersonGender = GenderOptions.Male
            };

            PersonResponse personResponse = _peopleService.AddPerson(personAddRequest);

            PersonUpdateRequest? personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            Assert.Throws<ArgumentException>(() =>
            {
                _peopleService.UpdatePerson(personUpdateRequest);
            });
           
        }

        [Fact]
        public void UpdatePerson_ProperDetails()
        {
            CountryAddRequest country = new CountryAddRequest()
            {
                CountryName = "Brasil"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(country);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                CountryID = countryResponse.CountryId,
                PersonName = "Guilherme",
                PersonEmail = "guimarkes457@gmail.com",
                DateOfBirth = DateTime.Parse("2006-03-12"),
                ReceiveNewsLetters = true,
                PersonGender = GenderOptions.Male,
                PersonAddress = "rua nao sei la das quantas"
                
            };

            PersonResponse personResponse = _peopleService.AddPerson(personAddRequest);

            PersonUpdateRequest? personUpdateRequest = personResponse.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = "Lindo";
            personUpdateRequest.PersonEmail = "lindo@gmail.com";

            PersonResponse personResponseFromUpdate = _peopleService.UpdatePerson(personUpdateRequest);

            PersonResponse personResponseFromGet = _peopleService.GetPersonById(personResponseFromUpdate.PersonId);

            Assert.Equal(personResponseFromUpdate, personResponseFromGet);

        }

        #endregion

        #region DeletePerson
  
        [Fact]
        public void DeletePerson_ValidPersonID()
        {
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                PersonEmail = "smith@example.com",
                PersonGender = GenderOptions.Male,
                PersonAddress = "address of smith",
                CountryID = country_response_1.CountryId,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters = true
            };
           
            PersonResponse person_response = _peopleService.AddPerson(person_request_1);

            bool isDeleted = _peopleService.DeletePerson(person_response.PersonId);

            Assert.True(isDeleted);
        }

        //If supplied id doesn't existis, it returns false
        [Fact]
        public void DeletePerson_inValidPersonID()
        {

            bool isDeleted = _peopleService.DeletePerson(Guid.NewGuid());

            Assert.False(isDeleted);
        }

        #endregion
    }
}
