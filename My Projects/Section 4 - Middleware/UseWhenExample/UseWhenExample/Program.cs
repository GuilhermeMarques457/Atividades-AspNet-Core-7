var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWhen(context => context.Request.Query.ContainsKey("username"),
    app => {
        app.Use(async (context, next) =>
        {
            await context.Response
                .WriteAsync("Hello from midleware branch");
            await next();
        }); 
    });

app.Run( async context =>
    await context.Response.WriteAsync("Hello from midleware branch")
);

app.Run();
