using System.ServiceModel;
using BusESB.Clientes;

namespace BusESB.Clientes
{
    public class PasajerosSoapClient : IPasajerosSoapClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PasajerosSoapClient> _logger;

        public PasajerosSoapClient(
            IConfiguration configuration,
            ILogger<PasajerosSoapClient> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public PasajeroClienteDto ValidarPasajero(string documento)
        {
            var url = _configuration["ServiciosSoap:ServicioPasajeros"]
                ?? throw new InvalidOperationException("URL de ServicioPasajeros no configurada.");

            var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            var endpoint = new EndpointAddress(url);

            using var factory = new ChannelFactory<IServicioPasajerosClient>(binding, endpoint);
            var canal = factory.CreateChannel();

            try
            {
                _logger.LogInformation("BusESB -> ServicioPasajeros.ValidarPasajero({Documento})", documento);
                var resultado = canal.ValidarPasajero(documento);
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
