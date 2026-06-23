using System.ServiceModel;
using WebCliente.Clientes;

namespace WebCliente.Servicios;

public class BusESBService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<BusESBService> _logger;

    public BusESBService(IConfiguration configuration, ILogger<BusESBService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public ResultadoConsultaViajeClienteDto ConsultarViaje(string documento, string origen, string destino, string fechaViaje)
    {
        var url = _configuration["ServiciosSoap:BusESB"]
            ?? throw new InvalidOperationException("URL del BusESB no configurada.");

        var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
        var endpoint = new EndpointAddress(url);

        using var factory = new ChannelFactory<IBusESBClient>(binding, endpoint);
        var canal = factory.CreateChannel();

        _logger.LogInformation("WebCliente -> BusESB.ConsultarViaje({Documento}, {Origen}, {Destino}, {Fecha})",
            documento, origen, destino, fechaViaje);

        var resultado = canal.ConsultarViaje(documento, origen, destino, fechaViaje);

        ((ICommunicationObject)canal).Close();
        return resultado;
    }

    public ResultadoConfirmacionViajeClienteDto ConfirmarViaje(string documento, string origen, string destino, string fechaViaje)
    {
        var url = _configuration["ServiciosSoap:BusESB"]
            ?? throw new InvalidOperationException("URL del BusESB no configurada.");

        var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
        var endpoint = new EndpointAddress(url);

        using var factory = new ChannelFactory<IBusESBClient>(binding, endpoint);
        var canal = factory.CreateChannel();

        _logger.LogInformation("WebCliente -> BusESB.ConfirmarViaje({Documento}, {Origen}, {Destino}, {Fecha})",
            documento, origen, destino, fechaViaje);

        var resultado = canal.ConfirmarViaje(documento, origen, destino, fechaViaje);

        ((ICommunicationObject)canal).Close();
        return resultado;
    }

    public ResultadoCancelacionViajeClienteDto CancelarReserva(string documento, string origen, string destino, string fechaViaje)
    {
        var url = _configuration["ServiciosSoap:BusESB"]
            ?? throw new InvalidOperationException("URL del BusESB no configurada.");

        var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
        var endpoint = new EndpointAddress(url);

        using var factory = new ChannelFactory<IBusESBClient>(binding, endpoint);
        var canal = factory.CreateChannel();

        _logger.LogInformation("WebCliente -> BusESB.CancelarReserva({Documento}, {Origen}, {Destino}, {Fecha})",
            documento, origen, destino, fechaViaje);

        var resultado = canal.CancelarReserva(documento, origen, destino, fechaViaje);

        ((ICommunicationObject)canal).Close();
        return resultado;
    }
}