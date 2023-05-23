using Microsoft.AspNetCore.Mvc;
using ViewsExample.Models;



namespace ViewsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("home")]
        [Route("/")]
        public IActionResult Index()
        {
            ViewData["appTitle"] = "Seu pai";

            List<Person> people = new List<Person>()
            {
                new Person() { Name = "Guilherme", Description = null, Gender = Gender.Male },
                new Person() { Name = "Pedro", Description = "Nazista", Gender = Gender.Other },
                new Person() { Name = "Laine", Description = "Gay", Gender = Gender.Male },
                new Person() { Name = "João", Description = "Noia", Gender = Gender.Female },
            };

            //ViewData["people"] = people;
            ViewBag.people = people;

            //Retrive a list of Person to the view "Index"
            return View("Index", people); //Views/Home/Index.cshtml
            //return View("abc"); //abc.cshtml
        }

        [Route("details/{name}")]
        public IActionResult Details(string? name)
        {
            if (name == null)
            {
                return Content("Person name can't be null");
            }

            List<Person> people = new List<Person>()
            {
                new Person() { Name = "Guilherme", Description = null, Gender = Gender.Male },
                new Person() { Name = "Pedro", Description = "Nazista", Gender = Gender.Other },
                new Person() { Name = "Laine", Description = "Gay", Gender = Gender.Male },
                new Person() { Name = "João", Description = "Noia", Gender = Gender.Female },
            };

            Person matchingPerson = people.Where(temp => temp.Name == name).FirstOrDefault();
            return View("Details", matchingPerson);
        }

        [Route("person-with-product")]
        public IActionResult PersonWithProduct()
        {
            Person person = new Person()
            {
                Name = "Tonin",
                Gender= Gender.Male,
                Description = "Gostoso"
            };

            Product product = new Product()
            {
                Name = "Cafe",
                Price= 10.50,
                Description = "Black Coffee",
                Category = "Vicio",
                
            };

            PersonWithProductWrapperModel personWithProductWrapper = new PersonWithProductWrapperModel()
            {
                Person = person,
                Product = product
            };
            return View("PersonAndProduct", personWithProductWrapper);
        }

        [Route("products-all")]
        public IActionResult All()
        {
            return View();
            //Views/Products/All.cshtml
            //If this file above is not found the controller will search in the shared folder
            //Views/Shared/All.cshtml
        }
    }
}
