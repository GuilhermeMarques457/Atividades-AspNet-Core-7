using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Rotativa.AspNetCore.Options;
using System.IO;

namespace XUnit.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {

        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonsController> _logger;

        public PersonsController(IPersonService personService, ICountriesService countriesService, ILogger<PersonsController> logger)
        {
            _personService = personService;
            _countriesService = countriesService;
            _logger = logger;   
        }

        //localhost:8080/index
        //[Route("/index")]

        //persons/index
        //[Route("index")]
        //persons/index
        [Route("[action]")]
        [Route("/")]
        public async Task<IActionResult> Index(
            string? searchBy,
            string? searchString,
            string sortBy = nameof(PersonResponse.PersonName),
            SortOrderOptions sortOrder = SortOrderOptions.ASC
        )
        {
            _logger.LogInformation("Index ation of personsControler reached");
            _logger.LogDebug("searchBy: " + searchBy + ", searchString: " + searchString + ", sortOrder: " + sortOrder + ", sortyBy: " + sortBy);

            ViewBag.SearchFields = new Dictionary<string, string>
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.PersonEmail), "Person Email" },
                { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                { nameof(PersonResponse.PersonAddress), "Person Address" },
                { nameof(PersonResponse.PersonGender), "Person Gender" },
                { nameof(PersonResponse.CountryID), "Country" }

            };

            List<PersonResponse>? persons = new List<PersonResponse>();
            persons = await _personService.GetFilterdPeople(searchBy, searchString);
            
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;  
            
            //Sorting
            List<PersonResponse>? sortedPersons = _personService.GetSortedPeople(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;

            return View(sortedPersons);
        }

        //persons/create
        //[Route("create")]

        //persons/create
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(
                temp => new SelectListItem() 
                { 
                    Text = temp.CountryName, Value = temp.CountryId.ToString() 
                });


            return View(new PersonAddRequest());
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create(PersonAddRequest person)
        {
            if(!ModelState.IsValid)
            {
                List<CountryResponse> countries = await _countriesService.GetAllCountries();
                ViewBag.Countries = countries;
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList(); 

                return View(person);
            }

            PersonResponse personRespose = await _personService.AddPerson(person);

            return RedirectToAction("Index", "Persons");
        }

        [Route("[action]/{id:guid}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            PersonResponse? personResponse = await _personService.GetPersonById(id);

            if(personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest? personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(
                temp => new SelectListItem()
                {
                    Text = temp.CountryName,
                    Value = temp.CountryId.ToString()
                });


            if (personUpdateRequest == null) 
            {
                return RedirectToAction("index");
            }

            return View(personUpdateRequest);
        }

        [Route("[action]/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(PersonUpdateRequest person)
        {
            PersonResponse? matchingPerson = await _personService.GetPersonById(person.PersonId);

            if(matchingPerson == null )
            {
                return RedirectToAction("index");
            }

            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = await _countriesService.GetAllCountries();
                ViewBag.Countries = countries.Select(
                    temp => new SelectListItem()
                    {
                        Text = temp.CountryName,
                        Value = temp.CountryId.ToString()
                    });

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(person);

            }

            PersonResponse personRespose = await _personService.UpdatePerson(person);

            return RedirectToAction("index", "persons");
        }

        [Route("[action]/{id:guid}")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            PersonResponse? personResponse = await _personService.GetPersonById(id);

            if (personResponse == null)
            {
                return RedirectToAction("index");
            }

            return View(personResponse);
        }

        [Route("[action]/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Delete(PersonUpdateRequest? person)
        {
            PersonResponse? matchingPerson = await _personService.GetPersonById(person?.PersonId);

            if (matchingPerson == null)
            {
                return RedirectToAction("index");
            }

            bool deleted = await _personService.DeletePerson(person?.PersonId);

            if(deleted) 
            {
                return RedirectToAction("index", "persons");
            }

            return View();

        }

        [Route("[action]")]
        public async Task<IActionResult> PeoplePDF()
        {
            List<PersonResponse> people = await _personService.GetPersonList();

            return new ViewAsPdf("PeoplePDF", people, ViewData)
            {
                PageMargins = new Margins()
                {
                    Top = 20,
                    Left = 20,
                    Right = 20,
                    Bottom = 20,
                },
                PageOrientation = Orientation.Landscape
            };
        }

        [Route("[action]")]
        public async Task<IActionResult> PeopleCSV()
        {
            MemoryStream? memoryStream = await _personService.GetPeopleCVS();
            return File(memoryStream, "application/octet-stream", "people.csv");
        }

        [Route("[action]")]
        public async Task<IActionResult> PeopleExcel()
        {
            MemoryStream? memoryStream = await _personService.GetPeopleExcel();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "people.xlsx");
        }
    }
}
