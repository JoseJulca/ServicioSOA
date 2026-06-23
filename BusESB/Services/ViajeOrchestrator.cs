using BusESB.Clientes;

namespace BusESB.Services
{
    public class ViajeOrchestrator : IViajeOrchestrator
    {
        private readonly IPasajerosSoapClient _pasajerosClient;
        private readonly IRutasSoapClient _rutasClient;
        private readonly IDisponibilidadSoapClient _disponibilidadClient;
        private readonly ILogger<ViajeOrchestrator> _logger;

        public ViajeOrchestrator(
            IPasajerosSoapClient pasajerosClient,
            IRutasSoapClient rutasClient,
            IDisponibilidadSoapClient disponibilidadClient,
            ILogger<ViajeOrchestrator> logger)
        {
            _pasajerosClient = pasajerosClient;
            _rutasClient = rutasClient;
            _disponibilidadClient = disponibilidadClient;
            _logger = logger;
        }

        public ResultadoConsultaViajeDto ConsultarViaje(string documento, string origen, string destino, string fechaViaje)
        {
            _logger.LogInformation("Orquestando ConsultarViaje.");

            var validacion = ValidarPasajeroYRuta(documento, origen, destino);

            if (!validacion.EsValido)
            {
                return new ResultadoConsultaViajeDto
                {
                    Exito = false,
                    Mensaje = validacion.MensajeError
                };
            }

            var ruta = validacion.Ruta!;
            var pasajero = validacion.Pasajero!;

            var disponibilidad = _disponibilidadClient.ConsultarDisponibilidad(ruta.RutaId, fechaViaje);

            if (!disponibilidad.Encontrada)
            {
                return new ResultadoConsultaViajeDto
                {
                    Exito = false,
                    Mensaje = $"No hay registro de disponibilidad para esa ruta en la fecha {fechaViaje}."
                };
            }

            var tieneReservaPrevia = _disponibilidadClient.ExisteReserva(documento, ruta.RutaId, fechaViaje);

            return new ResultadoConsultaViajeDto
            {
                Exito = true,
                Mensaje = "Consulta realizada correctamente.",
                Pasajero = pasajero.NombreCompleto,
                EstadoPasajero = pasajero.Estado,
                Ruta = $"{ruta.Origen} - {ruta.Destino}",
                FechaViaje = disponibilidad.FechaViaje,
                CuposDisponibles = disponibilidad.CuposDisponibles,
                EstadoViaje = disponibilidad.EstadoViaje,
                TieneReservaPrevia = tieneReservaPrevia
            };
        }

        public ResultadoConfirmacionViajeDto ConfirmarViaje(string documento, string origen, string destino, string fechaViaje)
        {
            _logger.LogInformation("Orquestando ConfirmarViaje.");

            var validacion = ValidarPasajeroYRuta(documento, origen, destino);

            if (!validacion.EsValido)
            {
                return new ResultadoConfirmacionViajeDto
                {
                    Exito = false,
                    Mensaje = validacion.MensajeError
                };
            }

            var ruta = validacion.Ruta!;
            var pasajero = validacion.Pasajero!;

            var reserva = _disponibilidadClient.ReservarCupo(documento, ruta.RutaId, fechaViaje);

            if (!reserva.Exito)
            {
                return new ResultadoConfirmacionViajeDto
                {
                    Exito = false,
                    Mensaje = reserva.Mensaje
                };
            }

            return new ResultadoConfirmacionViajeDto
            {
                Exito = true,
                Mensaje = "Reserva confirmada correctamente.",
                Pasajero = pasajero.NombreCompleto,
                Ruta = $"{ruta.Origen} - {ruta.Destino}",
                FechaViaje = fechaViaje,
                CuposRestantes = reserva.CuposRestantes
            };
        }

        public ResultadoCancelacionViajeDto CancelarReserva(string documento, string origen, string destino, string fechaViaje)
        {
            _logger.LogInformation("Orquestando CancelarReserva.");

            var ruta = _rutasClient.ObtenerRuta(origen, destino);

            if (!ruta.Encontrada)
            {
                return new ResultadoCancelacionViajeDto
                {
                    Exito = false,
                    Mensaje = $"No existe una ruta activa entre '{origen}' y '{destino}'."
                };
            }

            var cancelacion = _disponibilidadClient.CancelarReserva(documento, ruta.RutaId, fechaViaje);

            if (!cancelacion.Exito)
            {
                return new ResultadoCancelacionViajeDto
                {
                    Exito = false,
                    Mensaje = cancelacion.Mensaje
                };
            }

            return new ResultadoCancelacionViajeDto
            {
                Exito = true,
                Mensaje = "Reserva cancelada correctamente.",
                Ruta = $"{ruta.Origen} - {ruta.Destino}",
                FechaViaje = fechaViaje,
                CuposRestantes = cancelacion.CuposRestantes
            };
        }

        private (bool EsValido, string MensajeError, PasajeroClienteDto? Pasajero, RutaClienteDto? Ruta)
            ValidarPasajeroYRuta(string documento, string origen, string destino)
        {
            var pasajero = _pasajerosClient.ValidarPasajero(documento);

            if (!pasajero.Encontrado)
            {
                return (false, $"Pasajero con documento '{documento}' no fue encontrado.", null, null);
            }

            if (pasajero.Estado != "Valido")
            {
                return (false, $"Pasajero '{pasajero.NombreCompleto}' tiene estado '{pasajero.Estado}', no puede viajar.", null, null);
            }

            var ruta = _rutasClient.ObtenerRuta(origen, destino);

            if (!ruta.Encontrada)
            {
                return (false, $"No existe una ruta activa entre '{origen}' y '{destino}'.", null, null);
            }

            if (ruta.Estado != "Activa")
            {
                return (false, $"La ruta '{origen} - {destino}' se encuentra inactiva.", null, null);
            }

            return (true, string.Empty, pasajero, ruta);
        }
    }
}
