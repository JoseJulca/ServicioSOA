using Microsoft.Data.SqlClient;
using ServicioRutas.Repositories;
using System.Data;

namespace ServicioRutas;

public class ServicioRutasImpl : IServicioRutas
{
    private readonly IRutaRepository _repository;
    private readonly ILogger<ServicioRutasImpl> _logger;

    public ServicioRutasImpl(IRutaRepository repository,ILogger<ServicioRutasImpl> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public RutaDto ObtenerRuta(string origen, string destino)
    {
        _logger.LogInformation("ObtenerRuta invocado: {Origen} -> {Destino}", origen, destino);

        if (string.IsNullOrWhiteSpace(origen) || string.IsNullOrWhiteSpace(destino))
        {
            return new RutaDto
            {
                Encontrada = false,
                RutaId = 0,
                Origen = origen,
                Destino = destino,
                Estado = "DatosInvalidos"
            };
        }

        return _repository.ObtenerRuta(origen.Trim(), destino.Trim());
    }
}