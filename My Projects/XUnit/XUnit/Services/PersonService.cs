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

        public PersonService(bool initialize = true)
        {
            _peopleList = new List<Person>();
            _countriesService = new CountriesService();

            if (initialize)
            {
                _peopleList.Add(new Person() { PersonId = Guid.Parse("8082ED0C-396D-4162-AD1D-29A13F929824"), PersonName = "Aguste", PersonEmail = "aleddy0@booking.com", DateOfBirth = DateTime.Parse("1993-01-02"), PersonGender = "Male", PersonAddress = "0858 Novick Terrace", ReceiveNewsLetters = false, CountryID = Guid.Parse("000C76EB-62E9-4465-96D1-2C41FDB64C3B") });

                _peopleList.Add(new Person() { PersonId = Guid.Parse("06D15BAD-52F4-498E-B478-ACAD847ABFAA"), PersonName = "Jasmina", PersonEmail = "jsyddie1@miibeian.gov.cn", DateOfBirth = DateTime.Parse("1991-06-24"), PersonGender = "Female", PersonAddress = "0742 Fieldstone Lane", ReceiveNewsLetters = true, CountryID = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F") });

                _peopleList.Add(new Person() { PersonId = Guid.Parse("D3EA677A-0F5B-41EA-8FEF-EA2FC41900FD"), PersonName = "Kendall", PersonEmail = "khaquard2@arstechnica.com", DateOfBirth = DateTime.Parse("1993-08-13"), PersonGender = "Male", PersonAddress = "7050 Pawling Alley", ReceiveNewsLetters = false, CountryID = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F") });

                _peopleList.Add(new Person() { PersonId = Guid.Parse("89452EDB-BF8C-4283-9BA4-8259FD4A7A76"), PersonName = "Kilian", PersonEmail = "kaizikowitz3@joomla.org", DateOfBirth = DateTime.Parse("1991-06-17"), PersonGender = "Male", PersonAddress = "233 Buhler Junction", ReceiveNewsLetters = true, CountryID = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F") });

                _peopleList.Add(new Person() { PersonId = Guid.Parse("F5BD5979-1DC1-432C-B1F1-DB5BCCB0E56D"), PersonName = "Dulcinea", PersonEmail = "dbus4@pbs.org", DateOfBirth = DateTime.Parse("1996-09-02"), PersonGender = "Female", PersonAddress = "56 Sundown Point", ReceiveNewsLetters = false, CountryID = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E") });

                _peopleList.Add(new Person() { PersonId = Guid.Parse("A795E22D-FAED-42F0-B134-F3B89B8683E5"), PersonName = "Corabelle", PersonEmail = "cadams5@t-online.de", DateOfBirth = DateTime.Parse("1993-10-23"), PersonGender = "Female", PersonAddress = "4489 Hazelcrest Place", ReceiveNewsLetters = false, CountryID = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E") });

                _peopleList.Add(new Person() { PersonId = Guid.Parse("3C12D8E8-3C1C-4F57-B6A4-C8CAAC893D7A"), PersonName = "Faydra", PersonEmail = "fbischof6@boston.com", DateOfBirth = DateTime.Parse("1996-02-14"), PersonGender = "Female", PersonAddress = "2010 Farragut Pass", ReceiveNewsLetters = true, CountryID = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E") });

                _peopleList.Add(new Person() { PersonId = Guid.Parse("7B75097B-BFF2-459F-8EA8-63742BBD7AFB"), PersonName = "Oby", PersonEmail = "oclutheram7@foxnews.com", DateOfBirth = DateTime.Parse("1992-05-31"), PersonGender = "Male", PersonAddress = "2 Fallview Plaza", ReceiveNewsLetters = false, CountryID = Guid.Parse("15889048-AF93-412C-B8F3-22103E943A6D") });

                _peopleList.Add(new Person() { PersonId = Guid.Parse("6717C42D-16EC-4F15-80D8-4C7413E250CB"), PersonName = "Seumas", PersonEmail = "ssimonitto8@biglobe.ne.jp", DateOfBirth = DateTime.Parse("1999-02-02"), PersonGender = "Male", PersonAddress = "76779 Norway Maple Crossing", ReceiveNewsLetters = false, CountryID = Guid.Parse("15889048-AF93-412C-B8F3-22103E943A6D") });

                _peopleList.Add(new Person() { PersonId = Guid.Parse("6E789C86-C8A6-4F18-821C-2ABDB2E95982"), PersonName = "Freemon", PersonEmail = "faugustin9@vimeo.com", DateOfBirth = DateTime.Parse("1996-04-27"), PersonGender = "Male", PersonAddress = "8754 Becker Street", ReceiveNewsLetters = false, CountryID = Guid.Parse("000C76EB-62E9-4465-96D1-2C41FDB64C3B") });

            }
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

            return ConvertPersonToPersonResponse(person);


        }

        public List<PersonResponse> GetPersonList()
        {
            return _peopleList.Select(p => ConvertPersonToPersonResponse(p)).ToList();
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
                case nameof(PersonResponse.PersonName):
                    matchingPeople = personList.Where
                        (p => !string.IsNullOrEmpty(p.PersonName)
                            ? p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true).ToList();
                    break;

                case nameof(PersonResponse.PersonEmail):
                    matchingPeople = personList.Where
                        (p => !string.IsNullOrEmpty(p.PersonEmail)
                            ? p.PersonEmail.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true).ToList();
                    break;

                case nameof(PersonResponse.PersonAddress):
                    matchingPeople = personList.Where
                        (p => !string.IsNullOrEmpty(p.PersonAddress)
                            ? p.PersonAddress.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true).ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    matchingPeople = personList.Where
                        (p => p.DateOfBirth != null
                            ? p.DateOfBirth.Value.ToString("dd MMMM yyyy")
                                .Contains(searchString, StringComparison.OrdinalIgnoreCase)
                            : true).ToList();
                    break;

                case nameof(PersonResponse.PersonGender):
                    matchingPeople = personList.Where
                        (p => !string.IsNullOrEmpty(p.PersonGender)
                            ? p.PersonGender.Equals(searchString, StringComparison.OrdinalIgnoreCase)
                            : true).ToList();
                    break;

                case nameof(PersonResponse.CountryID):
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

            return ConvertPersonToPersonResponse(matchingPerson);
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
