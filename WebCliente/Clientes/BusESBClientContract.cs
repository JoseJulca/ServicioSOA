using System.Runtime.Serialization;
using System.ServiceModel;

namespace WebCliente.Clientes;

// =========================================================================
// CONTRATO "ESPEJO" del BusESB, usado por la WebCliente como CLIENTE SOAP.
// Mismo namespace XML que IBusESB en el proyecto BusESB.
// =========================================================================

[ServiceContract(Namespace = "http://soa.demo/bus")]
public interface IBusESBClient
{
    [OperationContract(Action = "http://soa.demo/bus/IBusESB/ConsultarViaje",
                        ReplyAction = "http://soa.demo/bus/IBusESB/ConsultarViajeResponse")]
    ResultadoConsultaViajeClienteDto ConsultarViaje(string documento, string origen, string destino, string fechaViaje);

    [OperationContract(Action = "http://soa.demo/bus/IBusESB/ConfirmarViaje",
                        ReplyAction = "http://soa.demo/bus/IBusESB/ConfirmarViajeResponse")]
    ResultadoConfirmacionViajeClienteDto ConfirmarViaje(string documento, string origen, string destino, string fechaViaje);

    [OperationContract(Action = "http://soa.demo/bus/IBusESB/CancelarReserva",
                        ReplyAction = "http://soa.demo/bus/IBusESB/CancelarReservaResponse")]
    ResultadoCancelacionViajeClienteDto CancelarReserva(string documento, string origen, string destino, string fechaViaje);
}

[DataContract(Namespace = "http://soa.demo/bus")]
public class ResultadoConsultaViajeClienteDto
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
public class ResultadoConfirmacionViajeClienteDto
{
    [DataMember] public bool Exito { get; set; }
    [DataMember] public string Mensaje { get; set; } = string.Empty;
    [DataMember] public string Pasajero { get; set; } = string.Empty;
    [DataMember] public string Ruta { get; set; } = string.Empty;
    [DataMember] public string FechaViaje { get; set; } = string.Empty;
    [DataMember] public int CuposRestantes { get; set; }
}

[DataContract(Namespace = "http://soa.demo/bus")]
public class ResultadoCancelacionViajeClienteDto
{
    [DataMember] public bool Exito { get; set; }
    [DataMember] public string Mensaje { get; set; } = string.Empty;
    [DataMember] public string Ruta { get; set; } = string.Empty;
    [DataMember] public string FechaViaje { get; set; } = string.Empty;
    [DataMember] public int CuposRestantes { get; set; }
}