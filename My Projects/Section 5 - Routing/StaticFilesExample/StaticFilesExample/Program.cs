using Microsoft.Extensions.FileProviders;

//to create a custom root folder
var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    WebRootPath = "myroot"
    
});
var app = builder.Build();
 
app.UseStaticFiles();

//To create multiple wwwroot folder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "mywebroot"))
});

app.MapGet("/", () => "Hello World!");

app.Run();
