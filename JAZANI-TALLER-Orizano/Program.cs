using Autofac.Extensions.DependencyInjection;
using Autofac;
using Jazani.Taller.Aplication.Cores;
using Jazani.Taller.Infrastructure.Cores.Context;
using JAZANI_TALLER_Orizano.Middlewares;
using JAZANI_TALLER_Orizano.Filters;
using Serilog.Events;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// utilizando la biblioteca Serilog en una aplicación .NET Core. Serilog es una biblioteca de registro
// altamente configurable que permite redirigir los registros a diferentes destinos,
// como la consola, archivos de registro, bases de datos y servicios de terceros.
var logger = new LoggerConfiguration()
    .WriteTo.Console(LogEventLevel.Information)
    .WriteTo.File(
        ".." + Path.DirectorySeparatorChar + "logapi.log",
        LogEventLevel.Warning,
        rollingInterval: RollingInterval.Day
    )
    .CreateLogger();

builder.Logging.AddSerilog(logger);


// Add services to the container.

builder.Services.AddControllers(options =>
{
    //para q reconosca el filtro de validacion
    options.Filters.Add(new ValidationFilter());
});
// Route Options(modelo maduracion)
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;// urls minuscula
    options.LowercaseQueryStrings = true;

});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//añadir los sevicios que estan en la cap 
// Infrastructure
builder.Services.addInfrastructureServices(builder.Configuration);

// Applications
builder.Services.AddApplicationServices();

//Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(options =>
    {
        options.RegisterModule(new InfraestructureAutofacModule());
        options.RegisterModule(new ApplicationAutoFacModule());
    });


// API-registrar Middlewares
builder.Services.AddTransient<ExceptionMiddleware>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//para que lo use la applicacion
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
