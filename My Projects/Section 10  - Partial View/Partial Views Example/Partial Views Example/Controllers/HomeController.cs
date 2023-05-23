using Microsoft.AspNetCore.Mvc;
using Partial_Views_Example.Models;

namespace Partial_Views_Example.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            //ViewBag.ListTitle = "Cities";
            //ViewBag.ListItems = new List<string>() 
            //{ 
            //    "Paris",
            //    "New York",
            //    "Dubai",
            //    "Adamantina"
            //};


            ListModel listModel = new ListModel();
            listModel.ListTitle = "Countries";
            listModel.ListItems = new List<string>()
            {
                 "Pracinha",
                 "Lucelia",
                 "Florida",
             };

            return View(listModel);
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("ProgrammingLanguages")]
        public IActionResult ProgrammingLanguages()
        {
            ListModel listModel = new ListModel() {
                ListTitle = "Programming languages list",
                ListItems = new List<string>()
                {
                    "Python",
                    "C#",
                    "Go"
                }
            
            };

            //return new PartialViewResult()
            //{
            //    ViewName = "_ListPartialView",
            //    Model = listModel
            //};

            return PartialView("_ListPartialView", listModel);
         
        }
    }
}
