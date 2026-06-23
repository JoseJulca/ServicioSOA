namespace ServicioPasajeros.Repositories
{
    public interface IPasajeroRepository
    {
        PasajeroDto ValidarPasajero(string documento);
    }
}
