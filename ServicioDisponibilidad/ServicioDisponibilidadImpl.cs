using Microsoft.Data.SqlClient;
using ServicioDisponibilidad.Repositories;
using ServicioDisponibilidad.Services;
using System.Data;
using System.Globalization;

namespace ServicioDisponibilidad;

public class ServicioDisponibilidadImpl : IServicioDisponibilidad
{
    private readonly IDisponibilidadRepository _disponibilidadRepository;
    private readonly IReservaRepository _reservaRepository;
    private readonly IFechaService _fechaService;
    private readonly ILogger<ServicioDisponibilidadImpl> _logger;

    public ServicioDisponibilidadImpl(
        IDisponibilidadRepository disponibilidadRepository,
        IReservaRepository reservaRepository,
        IFechaService fechaService,
        ILogger<ServicioDisponibilidadImpl> logger)
    {
        _disponibilidadRepository = disponibilidadRepository;
        _reservaRepository = reservaRepository;
        _fechaService = fechaService;
        _logger = logger;
    }

    public DisponibilidadDto ConsultarDisponibilidad(int rutaId, string fechaViaje)
    {
        _logger.LogInformation("ConsultarDisponibilidad invocado: RutaId={RutaId}, Fecha={Fecha}", rutaId, fechaViaje);

        if (rutaId <= 0)
        {
            return new DisponibilidadDto
            {
                Encontrada = false,
                RutaId = rutaId,
                FechaViaje = fechaViaje,
                CuposDisponibles = 0,
                EstadoViaje = "RutaInvalida"
            };
        }

        if (!_fechaService.TryParseFecha(fechaViaje, out var fecha))
        {
            return new DisponibilidadDto
            {
                Encontrada = false,
                RutaId = rutaId,
                FechaViaje = fechaViaje,
                CuposDisponibles = 0,
                EstadoViaje = "FechaInvalida"
            };
        }

        return _disponibilidadRepository.ConsultarDisponibilidad(rutaId, fecha, fechaViaje);
    }

    public ReservaDto ReservarCupo(string documento, int rutaId, string fechaViaje)
    {
        _logger.LogInformation("ReservarCupo invocado: Documento={Documento}, RutaId={RutaId}, Fecha={Fecha}", documento, rutaId, fechaViaje);

        if (string.IsNullOrWhiteSpace(documento) || rutaId <= 0)
        {
            return new ReservaDto { Exito = false, Mensaje = "Datos inválidos para reservar.", CuposRestantes = 0 };
        }

        if (!_fechaService.TryParseFecha(fechaViaje, out var fecha))
        {
            return new ReservaDto { Exito = false, Mensaje = "Fecha de viaje inválida.", CuposRestantes = 0 };
        }

        return _reservaRepository.ReservarCupo(documento.Trim(), rutaId, fecha);
    }

    public ReservaDto CancelarReserva(string documento, int rutaId, string fechaViaje)
    {
        _logger.LogInformation("CancelarReserva invocado: Documento={Documento}, RutaId={RutaId}, Fecha={Fecha}", documento, rutaId, fechaViaje);

        if (string.IsNullOrWhiteSpace(documento) || rutaId <= 0)
        {
            return new ReservaDto { Exito = false, Mensaje = "Datos inválidos para cancelar.", CuposRestantes = 0 };
        }

        if (!_fechaService.TryParseFecha(fechaViaje, out var fecha))
        {
            return new ReservaDto { Exito = false, Mensaje = "Fecha de viaje inválida.", CuposRestantes = 0 };
        }

        return _reservaRepository.CancelarReserva(documento.Trim(), rutaId, fecha);
    }

    public bool ExisteReserva(string documento, int rutaId, string fechaViaje)
    {
        _logger.LogInformation("ExisteReserva invocado: Documento={Documento}, RutaId={RutaId}, Fecha={Fecha}", documento, rutaId, fechaViaje);

        if (string.IsNullOrWhiteSpace(documento) || rutaId <= 0)
        {
            return false;
        }

        if (!_fechaService.TryParseFecha(fechaViaje, out var fecha))
        {
            return false;
        }

        return _reservaRepository.ExisteReserva(documento.Trim(), rutaId, fecha);
    }
}