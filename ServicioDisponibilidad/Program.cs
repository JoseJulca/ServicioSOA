using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Configuration;
using CoreWCF.Description;
using ServicioDisponibilidad;
using ServicioDisponibilidad.Services;
using ServicioDisponibilidad.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

builder.Services.AddTransient<IDisponibilidadRepository, DisponibilidadRepository>();
builder.Services.AddTransient<IReservaRepository, ReservaRepository>();
builder.Services.AddTransient<IFechaService, FechaService>();
builder.Services.AddTransient<ServicioDisponibilidadImpl>();

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<ServicioDisponibilidadImpl>(serviceOptions =>
    {
        serviceOptions.DebugBehavior.IncludeExceptionDetailInFaults = true;
    });

    serviceBuilder.AddServiceEndpoint<ServicioDisponibilidadImpl, IServicioDisponibilidad>(
        new BasicHttpBinding(BasicHttpSecurityMode.None),
        "/ServicioDisponibilidad.svc");
});

app.Services.GetRequiredService<ServiceMetadataBehavior>().HttpGetEnabled = true;

app.MapGet("/health", () => Results.Ok(new { status = "ServicioDisponibilidad OK" }));

app.Run();
