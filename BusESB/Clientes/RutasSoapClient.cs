using System.ServiceModel;
using BusESB.Clientes;

namespace BusESB.Clientes
{
    public class RutasSoapClient: IRutasSoapClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RutasSoapClient> _logger;

        public RutasSoapClient(
            IConfiguration configuration,
            ILogger<RutasSoapClient> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public RutaClienteDto ObtenerRuta(string origen, string destino)
        {
            var url = _configuration["ServiciosSoap:ServicioRutas"]
                ?? throw new InvalidOperationException("URL de ServicioRutas no configurada.");

            var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            var endpoint = new EndpointAddress(url);

            using var factory = new ChannelFactory<IServicioRutasClient>(binding, endpoint);
            var canal = factory.CreateChannel();

            try
            {
                _logger.LogInformation("BusESB -> ServicioRutas.ObtenerRuta({Origen}, {Destino})", origen, destino);
                var resultado = canal.ObtenerRuta(origen, destino);

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
