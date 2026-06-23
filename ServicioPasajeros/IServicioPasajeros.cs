using System.Runtime.Serialization;
using CoreWCF;

namespace ServicioPasajeros;

[ServiceContract(Namespace = "http://soa.demo/pasajeros")]
public interface IServicioPasajeros
{
    [OperationContract]
    PasajeroDto ValidarPasajero(string documento);
}

// DataContract = define como se serializa la entidad dentro del XML SOAP
[DataContract(Namespace = "http://soa.demo/pasajeros")]
public class PasajeroDto
{
    [DataMember] public bool Encontrado { get; set; }
    [DataMember] public string Documento { get; set; } = string.Empty;
    [DataMember] public string NombreCompleto { get; set; } = string.Empty;
    [DataMember] public string Estado { get; set; } = string.Empty; // Valido / Invalido / Bloqueado
}
