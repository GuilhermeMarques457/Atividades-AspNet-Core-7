using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Entities;
using RepositoryContracts;
using Repository;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Logging configurations
//builder.Host.ConfigureLogging(loggingProvider =>
//{
//    loggingProvider.ClearProviders();
//    loggingProvider.AddConsole();
//    loggingProvider.AddEventLog();
//});

//Seting Serilog up
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration) //Reading the appsettings
        .ReadFrom.Services(services); //Reads our service and make them available to serilog

});

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();

//By default AddDbContext is defined as a scoped service
builder.Services.AddDbContext<AspNetDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration!.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPropertiesAndHeaders | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;

});

var app = builder.Build();

app.UseSerilogRequestLogging();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//Enable httplog on our application
app.UseHttpLogging();

//app.Logger.LogDebug("Debug-message");
//app.Logger.LogInformation("Information-message");
//app.Logger.LogWarning("Warning-message");
//app.Logger.LogError("Error-message");
//app.Logger.LogCritical("Critical-message");

if(builder.Environment.IsEnvironment("Test") == false)
{
    //To setup the rotativa library
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}


app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();


//To access the auto-generated program class programmatically,
//This always exists but now we're saying explicty,
//therefore we can use the class anywhere
public partial class Program { }
