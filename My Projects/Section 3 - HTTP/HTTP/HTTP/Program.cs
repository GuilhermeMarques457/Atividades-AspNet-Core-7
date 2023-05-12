using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

app.Run(async (HttpContext context) =>
{
    //if(context.Request.Method == "GET")
    //{
    //    if (context.Request.Query.ContainsKey("id"))
    //    {
    //        string? id = context.Request.Query["id"];
    //        await context.Response.WriteAsync("ID: " + id);
    //    }
    //}
    //if (context.Request.Headers.ContainsKey("User-Agent"))
    //{

    //    string? userAgent = context.Request.Headers["User-Agent"];
    //    await context.Response.WriteAsync("User-Agent: " + userAgent);

    //}
    //if (context.Request.Headers.ContainsKey("AuthorizeKey"))
    //{

    //    string? AuthorizeKey = context.Request.Headers["AuthorizeKey"];
    //    await context.Response.WriteAsync("AuthorizeKey: " + AuthorizeKey);

    //}

    //string path = context.Request.Path;
    //string method = context.Request.Method;
    //context.Response.ContentType = "text/plain";
    //context.Response.Headers["MyKey"] = "My VALUE";
    //context.Response.StatusCode = 400;
    //await context.Response.WriteAsync("<h1>Bad Request Bro</h1>");
    //await context.Response.WriteAsync("Path: " + path);
    //await context.Response.WriteAsync("Method: " + method);

    System.IO.StreamReader reader = 
        new System.IO.StreamReader(context.Request.Body);

    string body = await reader.ReadToEndAsync();

    Dictionary<string, StringValues> keyValuePairs = 
        Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);

    if (keyValuePairs.ContainsKey("firstName"))
    {
        string? firstName = keyValuePairs["firstName"][0];
        await context.Response.WriteAsync(firstName);
    }

});

app.Run();
