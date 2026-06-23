using System.Runtime.Serialization;
using CoreWCF;

namespace ServicioRutas;

[ServiceContract(Namespace = "http://soa.demo/rutas")]
public interface IServicioRutas
{
    [OperationContract]
    RutaDto ObtenerRuta(string origen, string destino);
}

[DataContract(Namespace = "http://soa.demo/rutas")]
public class RutaDto
{
    [DataMember] public bool Encontrada { get; set; }
    [DataMember] public int RutaId { get; set; }
    [DataMember] public string Origen { get; set; } = string.Empty;
    [DataMember] public string Destino { get; set; } = string.Empty;
    [DataMember] public string Estado { get; set; } = string.Empty; // Activa / Inactiva
}
