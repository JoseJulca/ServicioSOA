using BusESB;
using BusESB.Clientes;
using BusESB.Services;
using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Configuration;
using CoreWCF.Description;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

// Inyección de dependencias del BusESB
builder.Services.AddTransient<BusESBImpl>();

builder.Services.AddTransient<IViajeOrchestrator, ViajeOrchestrator>();

builder.Services.AddTransient<IPasajerosSoapClient, PasajerosSoapClient>();
builder.Services.AddTransient<IRutasSoapClient, RutasSoapClient>();
builder.Services.AddTransient<IDisponibilidadSoapClient, DisponibilidadSoapClient>();

// Habilitar CORS para que la WebCliente (Razor Pages) pueda invocar al BUS
// si en algun momento se prueba con llamadas desde el navegador.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<BusESBImpl>(serviceOptions =>
    {
        serviceOptions.DebugBehavior.IncludeExceptionDetailInFaults = true;
    });

    serviceBuilder.AddServiceEndpoint<BusESBImpl, IBusESB>(
        new BasicHttpBinding(BasicHttpSecurityMode.None),
        "/BusESB.svc");
});

app.Services.GetRequiredService<ServiceMetadataBehavior>().HttpGetEnabled = true;

app.MapGet("/health", () => Results.Ok(new { status = "BusESB OK" }));

app.Run();
