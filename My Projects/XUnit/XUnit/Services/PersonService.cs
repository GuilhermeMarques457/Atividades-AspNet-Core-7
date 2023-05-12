using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _peopleList;
        private readonly ICountriesService _countriesService;

        public PersonService()
        {
            _peopleList = new List<Person>();
            _countriesService = new CountriesService();
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.CountryName = _countriesService
                .GetCountryById(personResponse.CountryID)?.CountryName;

            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            switch (personAddRequest)
            {
                case null:
                    throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Model validation made by me
            ValidationHelper.ModelValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();
            person.PersonId = Guid.NewGuid();

            _peopleList.Add(person);

            return ConvertPersonToPersonResponse(person);
        }
         
        public PersonResponse? GetPersonById(Guid? PersonId)
        {
            if (PersonId == null) return null;

            Person? person = _peopleList.FirstOrDefault(c => c.PersonId == PersonId);

            if (person == null) return null;

            return person.ToPersonResponse();


        }

        public List<PersonResponse> GetPersonList()
        {
            return _peopleList.Select(p => p.ToPersonResponse()).ToList();
        }

       
        public List<PersonResponse>? GetFilterdPeople(string searchBy, string? searchString)
        {
            List<PersonResponse>? personList = GetPersonList();
            List<PersonResponse>? matchingPeople = personList;
            if(string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return matchingPeople;
            };
            
            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    matchingPeople = personList.Where
                        (p => !string.IsNullOrEmpty(p.PersonName)
                            ? p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true).ToList();
                    break;

                case nameof(Person.PersonEmail):
                    matchingPeople = personList.Where
                        (p => !string.IsNullOrEmpty(p.PersonEmail)
                            ? p.PersonEmail.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true).ToList();
                    break;

                case nameof(Person.PersonAddress):
                    matchingPeople = personList.Where
                        (p => !string.IsNullOrEmpty(p.PersonAddress)
                            ? p.PersonAddress.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true).ToList();
                    break;

                case nameof(Person.DateOfBirth):
                    matchingPeople = personList.Where
                        (p => p.DateOfBirth != null
                            ? p.DateOfBirth.Value.ToString("dd MMMM yyyy")
                                .Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true).ToList();
                    break;

                case nameof(Person.PersonGender):
                    matchingPeople = personList.Where
                        (p => !string.IsNullOrEmpty(p.PersonGender)
                            ? p.PersonGender.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true).ToList();
                    break;

                case nameof(Person.CountryID):
                    matchingPeople = personList.Where
                        (p => !string.IsNullOrEmpty(p.CountryName)
                            ? p.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true).ToList();
                    break;

                default: matchingPeople = personList;
                    break;
            }
            return matchingPeople;
        }

        public List<PersonResponse> GetSortedPeople(List<PersonResponse> allPeople, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allPeople;

            List<PersonResponse> sortedPeople = (sortBy, sortOrder)
                switch
                {
                    (nameof(PersonResponse.PersonName), SortOrderOptions.ASC)
                        => allPeople.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
                       => allPeople.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonEmail), SortOrderOptions.ASC)
                        => allPeople.OrderBy(temp => temp.PersonEmail, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonEmail), SortOrderOptions.DESC)
                       => allPeople.OrderByDescending(temp => temp.PersonEmail, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonGender), SortOrderOptions.ASC)
                        => allPeople.OrderBy(temp => temp.PersonGender, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonGender), SortOrderOptions.DESC)
                       => allPeople.OrderByDescending(temp => temp.PersonGender).ToList(),

                    (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
                        => allPeople.OrderBy(temp => temp.DateOfBirth).ToList(),

                    (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
                       => allPeople.OrderByDescending(temp => temp.DateOfBirth.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonAge), SortOrderOptions.ASC)
                        => allPeople.OrderBy(temp => temp.PersonAge).ToList(),

                    (nameof(PersonResponse.PersonAge), SortOrderOptions.DESC)
                       => allPeople.OrderByDescending(temp => temp.PersonAge).ToList(),

                    (nameof(PersonResponse.PersonAddress), SortOrderOptions.ASC)
                        => allPeople.OrderBy(temp => temp.PersonAddress, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonAddress), SortOrderOptions.DESC)
                       => allPeople.OrderByDescending(temp => temp.PersonAddress, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.CountryName), SortOrderOptions.ASC)
                        => allPeople.OrderBy(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.CountryName), SortOrderOptions.DESC)
                       => allPeople.OrderByDescending(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC)
                    => allPeople.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

                    (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC)
                       => allPeople.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

                    _ => allPeople
                };

            return sortedPeople;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            //BECAREFULL WITH THIS
            if (personUpdateRequest == null)
                throw new ArgumentNullException(nameof(PersonUpdateRequest));
            
            ValidationHelper.ModelValidation(personUpdateRequest);

            Person? matchingPerson = _peopleList.FirstOrDefault(p => p.PersonId == personUpdateRequest.PersonId);

            if (matchingPerson == null) throw new ArgumentException("Given Id doesn't match with existing person");

            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.PersonGender = personUpdateRequest.PersonGender.ToString();
            matchingPerson.PersonEmail = personUpdateRequest.PersonEmail;
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.PersonAddress = personUpdateRequest.PersonAddress;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            return matchingPerson.ToPersonResponse();
        }

        public bool DeletePerson(Guid? PersonId)
        {
            if(PersonId == null)
            {
                throw new ArgumentNullException(nameof(PersonId));
            }
            Person? person = _peopleList.FirstOrDefault(p => p.PersonId == PersonId);

            if(person == null)
            {
                return false;
            }
            _peopleList.RemoveAll(person => person.PersonId == PersonId);

            return true;
        }
    }
}
