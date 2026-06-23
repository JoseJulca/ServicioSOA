namespace BusESB.Clientes
{
    public interface IPasajerosSoapClient
    {
        PasajeroClienteDto ValidarPasajero(string documento);
    }
}
