using Autofac;
using Autofac.Extensions.DependencyInjection;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//IoC container default from microsoft here
//builder.Services.Add(new ServiceDescriptor(
//    //Quando alguem chamar isso
//    typeof(ICitiesService),
//    //Criar um objeto disso
//    typeof(CitiesService),
//    ServiceLifetime.Scoped
//));

////Shorthand of the code above, equivalent
//builder.Services.AddTransient<ICitiesService, CitiesService>();
//builder.Services.AddScoped<ICitiesService, CitiesService>();

//IoC container from AUTOFAC
builder.Host.ConfigureContainer<ContainerBuilder>
    (containerBuilder =>
    {
        //Transient
        //containerBuilder.RegisterType<CitiesService>()
        //    .As<ICitiesService>().InstancePerDependency();

        //Scoped
        containerBuilder.RegisterType<CitiesService>()
            .As<ICitiesService>().InstancePerLifetimeScope();

        //Singleton
        //containerBuilder.RegisterType<CitiesService>()
        //    .As<ICitiesService>().SingleInstance();
    });

 
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
