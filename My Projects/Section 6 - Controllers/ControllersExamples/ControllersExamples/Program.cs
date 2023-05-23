using ControllersExamples.Controllers;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddTransient<HomeController>();

builder.Services.AddControllers();
//add all the controllers classes

var app = builder.Build();
//Enable static files to download
app.UseStaticFiles();
//Enaable routing and endPoints
app.MapControllers();

//app.UseRouting();
//app.UseEndpoints(endpoints =>
//{
//    //It will detect all the controllers in our project
//    //Enable the route for each action method
//    endpoints.MapControllers();
//});

app.Run();
