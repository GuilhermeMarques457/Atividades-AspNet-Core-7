using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Entities;
using RepositoryContracts;
using Repository;
using Serilog;
using CRUDExample.Filters.ActionFilters;
using SampleApplicationCRUD.Filters.ResultFilters;

var builder = WebApplication.CreateBuilder(args);

//Logging configurations
//builder.Host.ConfigureLogging(loggingProvider =>
//{
//    loggingProvider.ClearProviders();
//    loggingProvider.AddConsole();
//    loggingProvider.AddEventLog();
//});

//Setting Serilog up
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration) //Reading the appsettings
        .ReadFrom.Services(services); //Reads our service and make them available to serilog

});


//To add a global filter
builder.Services.AddControllersWithViews(options =>
{
    //To add without parameters
    //options.Filters.Add<ResponseHeaderActionFilter>();

    //To get the ILogger Service
    var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

    //To add parameters
    options.Filters.Add(new ResponseHeaderActionFilter(logger, "MyGlobalKey", "MyGlobalValue", 2));
});

//Adding the dependency injection with inversion of control (creating a Ioc container)
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddTransient<PersonsListResultFilter>();

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
