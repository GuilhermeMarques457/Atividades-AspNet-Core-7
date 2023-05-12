using ModelValidationExample.CustomModelBinders;

var builder = WebApplication.CreateBuilder(args);
//add just the controllers
//builder.Services.AddControllers();
//add the custom model binding with providers
builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new PersonBinderProvider());
});
//to add xml readers instead of json
builder.Services.AddControllers().AddXmlDataContractSerializerFormatters();
var app = builder.Build(); 

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
