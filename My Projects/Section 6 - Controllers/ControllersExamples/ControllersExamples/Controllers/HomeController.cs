using ControllersExamples.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControllersExamples.Controllers
{
    [Controller]
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        [Route("HomeController/sayhello")]
        [Route("/")]
        public ContentResult Index()
        {
            return new ContentResult()
            {
                Content = "Hello content from index",
                ContentType = "text/plain"
            };
        }

        [Route("about")]
        public ContentResult About()
        {

            return Content("<h1>Welcome to about</h1>", "text/html");

            //Same thing
            //return new ContentResult()
            //{
            //    Content = "Hello content from About",
            //    ContentType= "text/plain"
            //};
        }

        [Route("contact/{mobile:regex(^\\d{{10}}$)}")]
        public string Contact()
        {
            return "Hello world contact";
        }

        [Route("json")]
        public JsonResult json()
        {
            var pessoa = new Pessoa()
            {
                ID = Guid.NewGuid(),
                Nome = "Guilherme",
                Sobrenome = "Marques",
                Age = 17
            };

            return Json(pessoa);
        }

        [Route("file-download-virtual")]
        public VirtualFileResult FileDownloadVirtual()
        {
            return new VirtualFileResult("/sample.pdf", "application/pdf");
            //return new file("/sample.pdf", "application/pdf");
        }

        [Route("file-download-physical")]
        public PhysicalFileResult FileDownloadPhysical()
        {
            return new PhysicalFileResult(@"C:\\Users\\Microsoft\\Documents\\Arquivos Programação\\Udemy AspNet.Core 6\\My Projects\\Section 5 - Controllers\\ControllersExamples\sample.pdf", "application/pdf");
            //return PhysicalFile(@"C:\\Users\\Microsoft\\Documents\\Arquivos Programação\\Udemy AspNet.Core 6\\My Projects\\Section 5 - Controllers\\ControllersExamples\sample.pdf", "application/pdf");
        }

        [Route("file-download-content")]
        public FileContentResult FileDownloadContent()
        {
            byte[] bytes = System.IO.File.ReadAllBytes(@"C:\\Users\\Microsoft\\Documents\\Arquivos Programação\\Udemy AspNet.Core 6\\My Projects\\Section 5 - Controllers\\ControllersExamples\sample.pdf");
            return new FileContentResult(bytes, "application/pdf");
            //return File(bytes, "application/pdf");
        }
    } 
}
