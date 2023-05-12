var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    // BindingAddress your end points

    endpoints.Map("map", async (context) =>
    {
        await context.Response.WriteAsync("In the map");
    });

    endpoints.MapGet("mapGet", async (context) =>
    {
        await context.Response.WriteAsync("In the mapGet");
    });

    endpoints.MapPost("mapPost", async (context) =>
    {
        await context.Response.WriteAsync("In the mapPost");
    });
});

app.Run();
