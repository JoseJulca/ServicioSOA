using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Configuration;
using CoreWCF.Description;
using ServicioRutas;
using ServicioRutas.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

builder.Services.AddTransient<IRutaRepository, RutaRepository>();
builder.Services.AddTransient<ServicioRutasImpl>();

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<ServicioRutasImpl>(serviceOptions =>
    {
        serviceOptions.DebugBehavior.IncludeExceptionDetailInFaults = true;
    });

    serviceBuilder.AddServiceEndpoint<ServicioRutasImpl, IServicioRutas>(
        new BasicHttpBinding(BasicHttpSecurityMode.None),
        "/ServicioRutas.svc");
});

app.Services.GetRequiredService<ServiceMetadataBehavior>().HttpGetEnabled = true;

app.MapGet("/health", () => Results.Ok(new { status = "ServicioRutas OK" }));

app.Run();