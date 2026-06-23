namespace ServicioRutas.Repositories
{
    public interface IRutaRepository
    {
        RutaDto ObtenerRuta(string origen, string destino);
    }
}
