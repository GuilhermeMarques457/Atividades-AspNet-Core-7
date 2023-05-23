var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWhen(context => context.Request.Query.ContainsKey("email") 
&& context.Request.Query.ContainsKey("password"),
    app => {
        app.Run(async (context) =>
        {
            if (context.Request.Query["email"] != "" 
             && context.Request.Query["password"] != "")
            {
                if (context.Request.Query["email"] == "admin@example.com"
                 && context.Request.Query["password"] == "admin1234")
                {
                    await context.Response.WriteAsync("Successfull login");
                }
                else
                {
                    await context.Response.WriteAsync("Invalid login");
                }
            }
            
        });
    });

app.Run(async (context) =>
{
    if (context.Request.Query.ContainsKey("password"))
    {
        await context.Response.WriteAsync("Invalid input for email");
    }
    if (context.Request.Query.ContainsKey("email"))
    {
        await context.Response.WriteAsync("Invalid input for password");
    }
});


app.Run();