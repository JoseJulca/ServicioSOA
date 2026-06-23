namespace ServicioDisponibilidad.Repositories
{
    public interface IDisponibilidadRepository
    {
        DisponibilidadDto ConsultarDisponibilidad(int rutaId, DateTime fechaViaje, string fechaOriginal);
    }
}
