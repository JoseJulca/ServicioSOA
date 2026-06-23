using BusESB.Services;

namespace BusESB;

public class BusESBImpl : IBusESB
{
    private readonly IViajeOrchestrator _orchestrator;
    private readonly ILogger<BusESBImpl> _logger;

    public BusESBImpl(IViajeOrchestrator orchestrator,ILogger<BusESBImpl> logger)
    {
        _orchestrator = orchestrator;
        _logger = logger;
    }

    public ResultadoConsultaViajeDto ConsultarViaje(string documento, string origen, string destino, string fechaViaje)
    {
        _logger.LogInformation("BusESB.ConsultarViaje recibido.");
        return _orchestrator.ConsultarViaje(documento, origen, destino, fechaViaje);
    }

    public ResultadoConfirmacionViajeDto ConfirmarViaje(string documento, string origen, string destino, string fechaViaje)
    {
        _logger.LogInformation("BusESB.ConfirmarViaje recibido.");
        return _orchestrator.ConfirmarViaje(documento, origen, destino, fechaViaje);
    }

    public ResultadoCancelacionViajeDto CancelarReserva(string documento, string origen, string destino, string fechaViaje)
    {
        _logger.LogInformation("BusESB.CancelarReserva recibido.");
        return _orchestrator.CancelarReserva(documento, origen, destino, fechaViaje);
    }
}