namespace ServicioDisponibilidad.Repositories
{
    public interface IReservaRepository
    {
        ReservaDto ReservarCupo(string documento, int rutaId, DateTime fechaViaje);
        ReservaDto CancelarReserva(string documento, int rutaId, DateTime fechaViaje);
        bool ExisteReserva(string documento, int rutaId, DateTime fechaViaje);
    }
}
