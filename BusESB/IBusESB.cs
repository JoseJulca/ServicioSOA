using System.Runtime.Serialization;
using CoreWCF;

namespace BusESB;

[ServiceContract(Namespace = "http://soa.demo/bus")]
public interface IBusESB
{
    [OperationContract]
    ResultadoConsultaViajeDto ConsultarViaje(string documento, string origen, string destino, string fechaViaje);

    [OperationContract]
    ResultadoConfirmacionViajeDto ConfirmarViaje(string documento, string origen, string destino, string fechaViaje);

    [OperationContract]
    ResultadoCancelacionViajeDto CancelarReserva(string documento, string origen, string destino, string fechaViaje);
}

[DataContract(Namespace = "http://soa.demo/bus")]
public class ResultadoConsultaViajeDto
{
    [DataMember] public bool Exito { get; set; }
    [DataMember] public string Mensaje { get; set; } = string.Empty;
    [DataMember] public string Pasajero { get; set; } = string.Empty;
    [DataMember] public string EstadoPasajero { get; set; } = string.Empty;
    [DataMember] public string Ruta { get; set; } = string.Empty;
    [DataMember] public string FechaViaje { get; set; } = string.Empty;
    [DataMember] public int CuposDisponibles { get; set; }
    [DataMember] public string EstadoViaje { get; set; } = string.Empty;
    [DataMember] public bool TieneReservaPrevia { get; set; }
}

[DataContract(Namespace = "http://soa.demo/bus")]
public class ResultadoConfirmacionViajeDto
{
    [DataMember] public bool Exito { get; set; }
    [DataMember] public string Mensaje { get; set; } = string.Empty;
    [DataMember] public string Pasajero { get; set; } = string.Empty;
    [DataMember] public string Ruta { get; set; } = string.Empty;
    [DataMember] public string FechaViaje { get; set; } = string.Empty;
    [DataMember] public int CuposRestantes { get; set; }
}

[DataContract(Namespace = "http://soa.demo/bus")]
public class ResultadoCancelacionViajeDto
{
    [DataMember] public bool Exito { get; set; }
    [DataMember] public string Mensaje { get; set; } = string.Empty;
    [DataMember] public string Ruta { get; set; } = string.Empty;
    [DataMember] public string FechaViaje { get; set; } = string.Empty;
    [DataMember] public int CuposRestantes { get; set; }
}