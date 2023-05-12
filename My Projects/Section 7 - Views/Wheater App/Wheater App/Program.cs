using CitiesService;
using ServiceContracts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICityWeather, WeatherService>();


var app = builder.Build();


app.UseStaticFiles();
app.MapControllers();
app.Run();
