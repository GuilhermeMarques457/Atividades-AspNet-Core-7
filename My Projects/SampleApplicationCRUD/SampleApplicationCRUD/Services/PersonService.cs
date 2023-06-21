using CsvHelper;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using OfficeOpenXml;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly AspNetDbContext _context;
        private readonly ICountriesService _countriesService;

        public PersonService(AspNetDbContext context, ICountriesService countriesService)
        {
            _context = context;
            _countriesService = countriesService;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            switch (personAddRequest)
            {
                case null:
                    throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Model validation made by me
            ValidationHelper.ModelValidation(personAddRequest);

            if(personAddRequest.ReceiveNewsLetters == null)
            {
                personAddRequest.ReceiveNewsLetters = false;
            }

            Person person = personAddRequest.ToPerson();
            person.PersonID = Guid.NewGuid();

            await _context.Persons.AddAsync(person);

            await _context.SaveChangesAsync();

            //Insert Procedure
            //_context.sp_InsertPerson(person);

            return person.ToPersonResponse();
        }
         
        public async Task<PersonResponse?> GetPersonById(Guid? PersonId)
        {
            if (PersonId == null) return null;

            Person? person = await _context.Persons.Include("Country").FirstOrDefaultAsync(c => c.PersonID == PersonId);

            if (person == null) return null;

            return person.ToPersonResponse();


        }

        public async Task<List<PersonResponse>> GetPersonList()
        {
            
            //To get informations about the foreign object based in the PK
            var personList = await _context.Persons.Include("Country").ToListAsync();

            //Get through LINQ
            return _context.Persons.Include("Country").Select(p => p.ToPersonResponse()).ToList();

            //Get through Procedure
            //return _context.sp_GetAllPeople().Select(p => p.ToPersonResponse()).ToList();
        }
       
        public async Task<List<PersonResponse>?> GetFilterdPeople(string? searchBy, string? searchString)
        {
            List<PersonResponse>? personList = await GetPersonList();
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

        public List<PersonResponse>? GetSortedPeople(List<PersonResponse>? allPeople, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allPeople;

            List<PersonResponse>? sortedPeople =  (sortBy, sortOrder)
                switch
                {
                    (nameof(PersonResponse.PersonName), SortOrderOptions.ASC)
                        => allPeople?.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
                       => allPeople?.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonEmail), SortOrderOptions.ASC)
                        => allPeople?.OrderBy(temp => temp.PersonEmail, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonEmail), SortOrderOptions.DESC)
                       => allPeople?.OrderByDescending(temp => temp.PersonEmail, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonGender), SortOrderOptions.ASC)
                        => allPeople?.OrderBy(temp => temp.PersonGender, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonGender), SortOrderOptions.DESC)
                       => allPeople?.OrderByDescending(temp => temp.PersonGender).ToList(),

                    (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
                        => allPeople?.OrderBy(temp => temp.DateOfBirth).ToList(),

                    (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
                       => allPeople?.OrderByDescending(temp => temp.DateOfBirth.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonAge), SortOrderOptions.ASC)
                        => allPeople?.OrderBy(temp => temp.PersonAge).ToList(),

                    (nameof(PersonResponse.PersonAge), SortOrderOptions.DESC)
                       => allPeople?.OrderByDescending(temp => temp.PersonAge).ToList(),

                    (nameof(PersonResponse.PersonAddress), SortOrderOptions.ASC)
                        => allPeople?.OrderBy(temp => temp.PersonAddress, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonAddress), SortOrderOptions.DESC)
                       => allPeople?.OrderByDescending(temp => temp.PersonAddress, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.CountryName), SortOrderOptions.ASC)
                        => allPeople?.OrderBy(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.CountryName), SortOrderOptions.DESC)
                       => allPeople?.OrderByDescending(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC)
                    => allPeople?.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

                    (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC)
                       => allPeople?.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

                    _ => allPeople
                } ;

            return sortedPeople;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            //BECAREFULL WITH THIS
            if (personUpdateRequest == null)
                throw new ArgumentNullException(nameof(PersonUpdateRequest));
            
            ValidationHelper.ModelValidation(personUpdateRequest);

            if (personUpdateRequest.ReceiveNewsLetters == null)
            {
                personUpdateRequest.ReceiveNewsLetters = false;
            }

            Person? matchingPerson = await _context.Persons.FirstOrDefaultAsync(p => p.PersonID == personUpdateRequest.PersonId);

            if (matchingPerson == null) throw new ArgumentException("Given Id doesn't match with existing person");

            //Only for this object the savechanges will affect the data in the database
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.PersonGender = personUpdateRequest.PersonGender.ToString();
            matchingPerson.PersonEmail = personUpdateRequest.PersonEmail;
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.PersonAddress = personUpdateRequest.PersonAddress;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            await _context.SaveChangesAsync();

            return matchingPerson.ToPersonResponse();
        }

        public async Task<bool> DeletePerson(Guid? PersonId)
        {
            if(PersonId == null)
            {
                throw new ArgumentNullException(nameof(PersonId));
            }
            Person? person = await _context.Persons.FirstOrDefaultAsync(p => p.PersonID == PersonId);

            if(person == null)
            {
                return false;
            }
            
            _context.Persons.Remove(person);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<MemoryStream> GetPeopleCVS()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);

            //PersonName, Email, ...
            csvWriter.WriteField(nameof(PersonResponse.PersonName));
            csvWriter.WriteField(nameof(PersonResponse.PersonEmail));
            csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
            csvWriter.WriteField(nameof(PersonResponse.PersonAge));
            csvWriter.WriteField(nameof(PersonResponse.PersonGender));
            csvWriter.WriteField(nameof(PersonResponse.CountryName));
            csvWriter.WriteField(nameof(PersonResponse.PersonAddress));
            csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
            csvWriter.NextRecord();

            List<PersonResponse> people = await _context.Persons.Include("Country").Select(p => p.ToPersonResponse()).ToListAsync();

            foreach(var person in people)
            {
                csvWriter.WriteField(person.PersonName);
                csvWriter.WriteField(person.PersonEmail);
                if (person.DateOfBirth.HasValue)
                {
                    csvWriter.WriteField(person.DateOfBirth.Value.ToString("dd-MM-yyyy"));
                }
                else
                {
                    csvWriter.WriteField("");
                }
                csvWriter.WriteField(person.PersonAge);
                csvWriter.WriteField(person.PersonGender);
                csvWriter.WriteField(person.CountryName);
                csvWriter.WriteField(person.PersonAddress);
                csvWriter.WriteField(person.ReceiveNewsLetters);

                csvWriter.NextRecord();
                csvWriter.Flush();
            }

            memoryStream.Position = 0;

            return memoryStream;
        }

        public async Task<MemoryStream> GetPeopleExcel()
        {
            MemoryStream memoryStream = new MemoryStream();

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PeopleSheet");
                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Person Email";
                worksheet.Cells["C1"].Value = "Date of Birth";
                worksheet.Cells["D1"].Value = "Age";
                worksheet.Cells["E1"].Value = "Gender";
                worksheet.Cells["F1"].Value = "Country";
                worksheet.Cells["G1"].Value = "Address";
                worksheet.Cells["H1"].Value = "Receive news Letters";

                using (ExcelRange headerCells = worksheet.Cells["A1:H1"])
                {
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headerCells.Style.Font.Bold = true;
                };

                int row = 2;

                List<PersonResponse>? people = await _context.Persons.Include("Country").Select(p => p.ToPersonResponse()).ToListAsync();

                foreach (PersonResponse person in people)
                {
                    worksheet.Cells["A" + row].Value = person.PersonName;
                    worksheet.Cells["B" + row].Value = person.PersonEmail;
                    if (person.DateOfBirth.HasValue)
                    {
                        worksheet.Cells["C" + row].Value = person.DateOfBirth.Value.ToString("dd-MM-yyyy");
                    };
                    worksheet.Cells["D" + row].Value = person.PersonAge;
                    worksheet.Cells["E" + row].Value = person.PersonGender;
                    worksheet.Cells["F" + row].Value = person.CountryName;
                    worksheet.Cells["G" + row].Value = person.PersonAddress;
                    worksheet.Cells["H" + row].Value = person.ReceiveNewsLetters;

                    row++;
                }

                worksheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
