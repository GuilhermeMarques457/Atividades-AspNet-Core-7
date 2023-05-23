using MiddlewareExample.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<CustomMiddleware>();
var app = builder.Build();


app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Hello world from use 1\n");
    await next(context);
});

//Use custommiddleware through extension class
app.UseCustomMiddleware();

//Use customMiddlware through a normal class, herding the IMiddleware interface
app.UseMiddleware<CustomMiddleware>();

//Use from a middleware class
app.UseHelloCustomMiddleware();



app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Hello world from use 2\n");
    await next(context);
});

app.Run( async(HttpContext context) =>
{
    await context.Response.WriteAsync("Hello world from run\n");
});

app.Run();
