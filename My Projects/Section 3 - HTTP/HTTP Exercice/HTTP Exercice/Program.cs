
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run(async (HttpContext context) =>
{
    if (context.Request.Query.ContainsKey("firstNumber") 
    && context.Request.Query.ContainsKey("secondNumber"))
    {
        int? firstNumber = Convert.ToInt32(context.Request.Query["firstNumber"]);
        int? secondNumber = Convert.ToInt32(context.Request.Query["secondNumber"]);
        double results;
        string? operation = context.Request.Query["operation"];

        if (operation == "divide")
        {
            results = Convert.ToDouble(firstNumber / secondNumber);
            await context.Response.WriteAsync(results.ToString());
        }
        else if (operation == "add")
        {
            results = Convert.ToDouble(firstNumber + secondNumber);
            await context.Response.WriteAsync(results.ToString());
        }
        else if(operation == "subtract")
        {
            results = Convert.ToDouble(firstNumber - secondNumber);
            await context.Response.WriteAsync(results.ToString());
        }
        else if (operation == "multiply")
        {
            results = Convert.ToDouble(firstNumber * secondNumber);
            await context.Response.WriteAsync(results.ToString());
        }
        else
        {
            await context.Response.WriteAsync("Invalid input for operation");
        }
    }
    else
    {
        await context.Response.WriteAsync("Invalid input for firstNumber");
        await context.Response.WriteAsync("Invalid input for secondNumber");
        await context.Response.WriteAsync("Invalid input for operation");
    }
    
});


app.Run();
