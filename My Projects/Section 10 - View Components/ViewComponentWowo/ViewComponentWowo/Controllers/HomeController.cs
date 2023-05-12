using Microsoft.AspNetCore.Mvc;
using ViewComponentWowo.Models;

namespace ViewComponentsExample.Controllers
{
  public class HomeController : Controller
  {
    [Route("/")]
    public IActionResult Index()
    {
      return View();
    }

    [Route("about")]
    public IActionResult About()
    {
      return View();
    }

    [Route("friends-list")]
    public IActionResult LoadFriendsList()
        {
        
                        PersonGridModel model2 = new PersonGridModel()
                        {
                            GridTitle = "Friends",
                            Persons = new List<Person>
                        {
                            new Person()
                            {
                                Name = "Joao", Job="Lazy"
                            },
                            new Person()
                            {
                                Name = "Andrey", Job="Buddy"
                            },
                            new Person()
                            {
                                Name = "Luloquinha", Job="Loyal"
                            },
                        }
                        };
            return ViewComponent("Grid", new { model = model2});
            
        }
  }
}
