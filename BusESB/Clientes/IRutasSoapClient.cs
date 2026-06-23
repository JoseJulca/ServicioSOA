namespace BusESB.Clientes
{
    public interface IRutasSoapClient
    {
        RutaClienteDto ObtenerRuta(string origen, string destino);
    }
}
