namespace ServicioDisponibilidad.Services
{
    public interface IFechaService
    {
        bool TryParseFecha(string fechaViaje, out DateTime fecha);
    }
}
