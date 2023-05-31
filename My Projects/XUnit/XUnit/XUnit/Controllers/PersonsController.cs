using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace XUnit.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {

        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;

        public PersonsController(IPersonService personService, ICountriesService countriesService)
        {
            _personService = personService;
            _countriesService = countriesService;
        }

        //localhost:8080/index
        //[Route("/index")]

        //persons/index
        //[Route("index")]
        //persons/index
        [Route("[action]")]
        [Route("/")]
        public IActionResult Index(
            string? searchBy,
            string? searchString,
            string sortBy = nameof(PersonResponse.PersonName),
            SortOrderOptions sortOrder = SortOrderOptions.ASC
        )
        {
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
            persons = _personService.GetFilterdPeople(searchBy, searchString);
            
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;  
            
            //Sorting
            List<PersonResponse> sortedPersons = _personService.GetSortedPeople(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;

            return View(sortedPersons);
        }

        //persons/create
        //[Route("create")]

        //persons/create
        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries;
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult Create(PersonAddRequest person)
        {
            if(!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList(); 

                return View();
            }

            PersonResponse personRespose = _personService.AddPerson(person);

            return RedirectToAction("index", "persons");
        }
    }
}
