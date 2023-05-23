using Microsoft.AspNetCore.Mvc;
using Services;
using ServiceContracts;
using Autofac;

namespace DIExample.Controllers
{
  public class HomeController : Controller
  {

        //Creating an object of cities service globaly
        private readonly ICitiesService _citiesService1;
        private readonly ICitiesService _citiesService2;
        private readonly ICitiesService _citiesService3;
        //private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILifetimeScope _lifeTimeScope;

        public HomeController(ICitiesService citiesService1, ICitiesService citiesService2, ICitiesService citiesService3, ILifetimeScope lifeTimeScope)
        {
            //It recieves the object from the IcitiesService which gets an object from the dependency (class model)
            _citiesService1 = citiesService1;
            _citiesService2 = citiesService2;
            _citiesService3 = citiesService3;
            _lifeTimeScope = lifeTimeScope;
        }


        //Creating just in the action method so it doesn't affect the other action methods
        [Route("/")]
        public IActionResult Index(/*[FromServices] ICitiesService _citiesService*/)
        {
            List<string> cities = _citiesService1.GetCities();
            ViewBag.IntanceId_CitiesService_1 = _citiesService1.ServiceInstanceId;
            ViewBag.IntanceId_CitiesService_2 = _citiesService2.ServiceInstanceId;
            ViewBag.IntanceId_CitiesService_3 = _citiesService3.ServiceInstanceId;

            //using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            //using (ILifetimeScope scope = _lifeTimeScope.BeginLifetimeScope())
            //{
            //    Inject CitiesService

            //    this will return an object of citiesService so we can work with database
            //    ICitiesService citiesService = scope.ServiceProvider.GetRequiredService<ICitiesService>();
            //    ICitiesService citiesService = scope.Resolve<ICitiesService>();
            //    dbwork

            //    ViewBag.IntanceId_CitiesService_InScope = citiesService.ServiceInstanceId;
            //}

            //When the work of the database is completed we close that database connection using the dispose method present in the object

            //endy of scope
            //_citiesService.Dispose()
            return View(cities);
        }
  }
}
