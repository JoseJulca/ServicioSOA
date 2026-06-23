using System.Data;
using Microsoft.Data.SqlClient;
using ServicioPasajeros.Repositories;

namespace ServicioPasajeros;

public class ServicioPasajerosImpl : IServicioPasajeros
{
    private readonly IPasajeroRepository _repository;
    private readonly ILogger<ServicioPasajerosImpl> _logger;

    public ServicioPasajerosImpl(IPasajeroRepository repository,ILogger<ServicioPasajerosImpl> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public PasajeroDto ValidarPasajero(string documento)
    {
        _logger.LogInformation("ValidarPasajero invocado con documento: {Documento}", documento);

        if (string.IsNullOrWhiteSpace(documento))
        {
            return new PasajeroDto
            {
                Encontrado = false,
                Documento = documento,
                NombreCompleto = string.Empty,
                Estado = "DocumentoInvalido"
            };
        }

        return _repository.ValidarPasajero(documento.Trim());
    }
}
