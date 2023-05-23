using Microsoft.AspNetCore.Http;
using RoutingExample.CustomConstraints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("months", typeof(MonthsCustomContraints));
});
var app = builder.Build();

//Just to enable "UseEndpoints"
//app.Use(async (context, next) =>
//{
//    Endpoint? end = context.GetEndpoint();
//    await next(context);
//});

app.UseRouting();

//app.Use(async (context, next) =>
//{
//    Endpoint? end = context.GetEndpoint();
//    await next(context);
//});

app.UseEndpoints(endpoints =>
{
    
    //It's not case sensitive kaakakak

    
    endpoints.Map("files/{fileName}.{fileExtension}", async context=>
    {
        string? fileName = Convert.ToString(context.Request.RouteValues["fileName"]);
        string? fileExtension = Convert.ToString(context.Request.RouteValues["fileExtension"]);
        await context.Response.WriteAsync($"in files, your file {fileName}.{fileExtension}");
    });

    //default parameter {parameter=defaultvalue}
    endpoints.Map("products/details/{id=1}", async context =>
    {
        int id = Convert.ToInt32(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"Product id={id}");
    });

    //default parameter {parameter=defaultvalue}
    endpoints.Map("cars/details/{id:int?}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            int id = Convert.ToInt32(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Car id={id}");
        }
        else
        {
            await context.Response.WriteAsync($"Car id is not supplied");
        }
       
    });

    endpoints.Map("cities/{cityid:guid}", async context =>
    {
        Guid cityid = Guid.Parse(context.Request.RouteValues["cityid"]!.ToString()!);

        await context.Response.WriteAsync($"City information id = {cityid}");
    });

    endpoints.Map("daily-report/{reportdate:datetime}", async context =>
    {
        DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);

        await context.Response.WriteAsync($"Daily report date: {reportDate}");
    });

    endpoints.Map("random-number/{num:minlength(3):maxlength(10)}", async context =>
    {
        double num = Convert.ToDouble(context.Request.RouteValues["reportdate"]);

        await context.Response.WriteAsync($"Daily report date: {num}");
    });

    endpoints.Map("sales-report/{year:int:min(1900):max(2023)}/{month:months}", async context =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = context.Request.RouteValues["month"]?.ToString();

        await context.Response.WriteAsync($"Daily report date: {year}/{month}");
    });

    // BindingAddress your end points

    //endpoints.Map("map", async (context) =>
    //{
    //    await context.Response.WriteAsync("In the map");
    //});

    //endpoints.MapGet("map/get", async (context) =>
    //{
    //    await context.Response.WriteAsync("In the mapGet");
    //});

    //endpoints.MapPost("map/post", async (context) =>
    //{
    //    await context.Response.WriteAsync("In the mapPost");
    //});

});

app.Run();
