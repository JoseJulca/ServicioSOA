namespace BusESB.Services
{
    public interface IViajeOrchestrator
    {
        ResultadoConsultaViajeDto ConsultarViaje(string documento, string origen, string destino, string fechaViaje);
        ResultadoConfirmacionViajeDto ConfirmarViaje(string documento, string origen, string destino, string fechaViaje);
        ResultadoCancelacionViajeDto CancelarReserva(string documento, string origen, string destino, string fechaViaje);
    }
}
