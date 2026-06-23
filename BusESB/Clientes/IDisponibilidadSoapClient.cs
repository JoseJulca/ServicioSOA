namespace BusESB.Clientes
{
    public interface IDisponibilidadSoapClient
    {
        DisponibilidadClienteDto ConsultarDisponibilidad(int rutaId, string fechaViaje);
        ReservaClienteDto ReservarCupo(string documento, int rutaId, string fechaViaje);
        ReservaClienteDto CancelarReserva(string documento, int rutaId, string fechaViaje);
        bool ExisteReserva(string documento, int rutaId, string fechaViaje);
    }
}
