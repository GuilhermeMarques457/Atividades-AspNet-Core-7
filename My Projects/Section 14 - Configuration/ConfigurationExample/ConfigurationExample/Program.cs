using ConfigurationExample;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
//hey builder creates a configuration object of the class WeatherApiOptions of the section weatherApi
//just like dependency injection we can call it in any constructor controller
builder.Services.Configure<WeatherApiOptions>(builder.Configuration.GetSection("weatherApi"));

//Load MyOwnConfig.json
builder.Configuration.AddJsonFile("MyOwnConfig.json", optional: true, reloadOnChange: true);
var app = builder.Build();

//app.Environment.IsProduction();
//app.Environment.IsStaging()
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
 