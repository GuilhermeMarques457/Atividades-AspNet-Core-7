var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Dictionary<int, string> countries = new Dictionary<int, string>() { };

countries.Add(1, "United States");
countries.Add(2, "Canada");
countries.Add(3, "United Kingdom");
countries.Add(4, "United India");
countries.Add(5, "Japan");

app.MapGet("/countries", async (HttpContext context) => 
{
    for(int i = 1;i <= countries.Count; i++)
    {
        await context.Response.WriteAsync(i + ", " + countries[i] +"\n");
    }
});

app.MapGet("/countries/{countryID:int:range(1,5)}", async (HttpContext context) =>
{
    int id = Convert.ToInt32(context.Request.RouteValues["countryID"]);
    await context.Response.WriteAsync(countries[id]);

});

app.MapGet("/countries/{countryID}", async (HttpContext context) =>
{
    await context.Response.WriteAsync("No Country");

});

app.MapGet("/countries/{countryID:length(3, 100000)}", async (HttpContext context) =>
{
    await context.Response.WriteAsync("The CountryID should be between 1 and 100");

});



app.Run();
