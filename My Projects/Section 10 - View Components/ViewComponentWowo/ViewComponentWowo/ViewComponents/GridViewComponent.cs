using Microsoft.AspNetCore.Mvc;
using ViewComponentWowo.Models;

namespace ViewComponentsExample.ViewComponents
{
    //Optional
    //[ViewComponent]
  public class GridViewComponent : ViewComponent
  {

        //The first thing that happen when we call a view component
        public async Task<IViewComponentResult> InvokeAsync(PersonGridModel model)
        {
            //PersonGridModel model = new PersonGridModel()
            //{
            //    GridTitle = "PersonList",
            //    Persons = new List<Person>
            //    {
            //        new Person()
            //        {
            //            Name = "Guilherme", Job="Programmer"
            //        },
            //        new Person()
            //        {
            //            Name = "Gustavo", Job="Sucker"
            //        },
            //        new Person()
            //        {
            //            Name = "Mama", Job="Linda"
            //        },
            //    }
            //};

            //ViewBag.Grid = model;

            return View(model); //Involking partial views
        }
  }
}
