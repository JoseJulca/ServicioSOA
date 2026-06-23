using System.ServiceModel;

namespace BusESB.Clientes
{
    public class DisponibilidadSoapClient : IDisponibilidadSoapClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DisponibilidadSoapClient> _logger;

        public DisponibilidadSoapClient(
            IConfiguration configuration,
            ILogger<DisponibilidadSoapClient> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }             

        public DisponibilidadClienteDto ConsultarDisponibilidad(int rutaId, string fechaViaje)
        {
            using var factory = new ChannelFactory<IServicioDisponibilidadClient>(
                new BasicHttpBinding(BasicHttpSecurityMode.None),
                new EndpointAddress(_configuration["ServiciosSoap:ServicioDisponibilidad"]
                    ?? throw new InvalidOperationException("URL de ServicioDisponibilidad no configurada.")));

            var canal = factory.CreateChannel();

            try
            {
                _logger.LogInformation(
                    "BusESB -> ServicioDisponibilidad.ConsultarDisponibilidad({RutaId}, {FechaViaje})",
                    rutaId,
                    fechaViaje);

                var resultado = canal.ConsultarDisponibilidad(rutaId, fechaViaje);

                ((ICommunicationObject)canal).Close();

                return resultado;
            }
            catch
            {
                ((ICommunicationObject)canal).Abort();
                throw;
            }
        }

        public ReservaClienteDto ReservarCupo(string documento, int rutaId, string fechaViaje)
        {
            using var factory = new ChannelFactory<IServicioDisponibilidadClient>(
                new BasicHttpBinding(BasicHttpSecurityMode.None),
                new EndpointAddress(_configuration["ServiciosSoap:ServicioDisponibilidad"]
                    ?? throw new InvalidOperationException("URL de ServicioDisponibilidad no configurada.")));

            var canal = factory.CreateChannel();

            try
            {
                _logger.LogInformation(
                    "BusESB -> ServicioDisponibilidad.ReservarCupo({Documento}, {RutaId}, {FechaViaje})",
                    documento,
                    rutaId,
                    fechaViaje);

                var resultado = canal.ReservarCupo(documento, rutaId, fechaViaje);

                ((ICommunicationObject)canal).Close();

                return resultado;
            }
            catch
            {
                ((ICommunicationObject)canal).Abort();
                throw;
            }
        }

        public ReservaClienteDto CancelarReserva(string documento, int rutaId, string fechaViaje)
        {
            using var factory = new ChannelFactory<IServicioDisponibilidadClient>(
                new BasicHttpBinding(BasicHttpSecurityMode.None),
                new EndpointAddress(_configuration["ServiciosSoap:ServicioDisponibilidad"]
                    ?? throw new InvalidOperationException("URL de ServicioDisponibilidad no configurada.")));

            var canal = factory.CreateChannel();

            try
            {
                _logger.LogInformation(
                    "BusESB -> ServicioDisponibilidad.CancelarReserva({Documento}, {RutaId}, {FechaViaje})",
                    documento,
                    rutaId,
                    fechaViaje);

                var resultado = canal.CancelarReserva(documento, rutaId, fechaViaje);

                ((ICommunicationObject)canal).Close();

                return resultado;
            }
            catch
            {
                ((ICommunicationObject)canal).Abort();
                throw;
            }
        }

        public bool ExisteReserva(string documento, int rutaId, string fechaViaje)
        {
            using var factory = new ChannelFactory<IServicioDisponibilidadClient>(
                new BasicHttpBinding(BasicHttpSecurityMode.None),
                new EndpointAddress(_configuration["ServiciosSoap:ServicioDisponibilidad"]
                    ?? throw new InvalidOperationException("URL de ServicioDisponibilidad no configurada.")));

            var canal = factory.CreateChannel();

            try
            {
                _logger.LogInformation(
                    "BusESB -> ServicioDisponibilidad.ExisteReserva({Documento}, {RutaId}, {FechaViaje})",
                    documento,
                    rutaId,
                    fechaViaje);

                var resultado = canal.ExisteReserva(documento, rutaId, fechaViaje);

                ((ICommunicationObject)canal).Close();

                return resultado;
            }
            catch
            {
                ((ICommunicationObject)canal).Abort();
                throw;
            }
        }
    }
}
