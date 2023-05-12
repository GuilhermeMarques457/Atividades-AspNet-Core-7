var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Hello world from use 1");
    await next(context);
});

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Hello world from use 2");
    await next(context);
});

app.Run( async(HttpContext context) =>
{
    await context.Response.WriteAsync("Hello world from run");
});

app.Run();
