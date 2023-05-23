using Microsoft.AspNetCore.Mvc;
using ModelValidationExample.CustomModelBinders;
using ModelValidationExample.Models;

namespace ModelValidationExample.Controllers
{
    public class PersonController : Controller
    {
        [Route("register")]
        //to bind especific properties
        //public IActionResult Index(
        //    [Bind
        //    (nameof(Person.Name), nameof(Person.Email),
        //    nameof(Person.Password),
        //    nameof(Person.ConfirmPassword))]
        //Person person)

        //to bind using custom binders, without creating a Provider
        //public IActionResult Index(
        //    [FromBody]
        //    [ModelBinder(BinderType = typeof(PersonModelBinder))]
        //    Person person)
        //{
        public IActionResult Index(Person person, [FromHeader(Name = "User-Agent")]string UserAgent)
        {
            if (!ModelState.IsValid)
            {
                List<string> errorsList = new List<string>();
                foreach(var values in ModelState.Values)
                {
                    foreach (var error in values.Errors)
                    {
                        errorsList.Add(error.ErrorMessage);
                       
                    }
                }
                string errors = string.Join("\n", errorsList    );
                return BadRequest(errors);
            }


            return Content($"{person}, {UserAgent}");
        }
    }
}
