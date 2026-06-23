using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using CoreWCF.Channels;
using ServicioPasajeros;
using ServicioPasajeros.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Servicios necesarios para hospedar SOAP con CoreWCF
builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

// Inyección de dependencias
builder.Services.AddTransient<IPasajeroRepository, PasajeroRepository>();
builder.Services.AddTransient<ServicioPasajerosImpl>();

var app = builder.Build();

// Habilita la generacion del WSDL en /ServicioPasajeros.svc?wsdl
app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<ServicioPasajerosImpl>(serviceOptions =>
    {
        serviceOptions.DebugBehavior.IncludeExceptionDetailInFaults = true;
    });

    serviceBuilder.AddServiceEndpoint<ServicioPasajerosImpl, IServicioPasajeros>(
        new BasicHttpBinding(BasicHttpSecurityMode.None),
        "/ServicioPasajeros.svc");
});

app.Services.GetRequiredService<ServiceMetadataBehavior>().HttpGetEnabled = true;

// Endpoint simple de salud para verificar que el contenedor esta vivo
app.MapGet("/health", () => Results.Ok(new { status = "ServicioPasajeros OK" }));

app.Run();
