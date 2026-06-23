using System.Runtime.Serialization;
using System.ServiceModel;

namespace BusESB.Clientes;

// ---- Cliente para ServicioPasajeros ----
[ServiceContract(Namespace = "http://soa.demo/pasajeros")]
public interface IServicioPasajerosClient
{
    [OperationContract(Action = "http://soa.demo/pasajeros/IServicioPasajeros/ValidarPasajero",
                        ReplyAction = "http://soa.demo/pasajeros/IServicioPasajeros/ValidarPasajeroResponse")]
    PasajeroClienteDto ValidarPasajero(string documento);
}

[DataContract(Namespace = "http://soa.demo/pasajeros")]
public class PasajeroClienteDto
{
    [DataMember] public bool Encontrado { get; set; }
    [DataMember] public string Documento { get; set; } = string.Empty;
    [DataMember] public string NombreCompleto { get; set; } = string.Empty;
    [DataMember] public string Estado { get; set; } = string.Empty;
}

// ---- Cliente para ServicioRutas ----
[ServiceContract(Namespace = "http://soa.demo/rutas")]
public interface IServicioRutasClient
{
    [OperationContract(Action = "http://soa.demo/rutas/IServicioRutas/ObtenerRuta",
                        ReplyAction = "http://soa.demo/rutas/IServicioRutas/ObtenerRutaResponse")]
    RutaClienteDto ObtenerRuta(string origen, string destino);
}

[DataContract(Namespace = "http://soa.demo/rutas")]
public class RutaClienteDto
{
    [DataMember] public bool Encontrada { get; set; }
    [DataMember] public int RutaId { get; set; }
    [DataMember] public string Origen { get; set; } = string.Empty;
    [DataMember] public string Destino { get; set; } = string.Empty;
    [DataMember] public string Estado { get; set; } = string.Empty;
}

// ---- Cliente para ServicioDisponibilidad ----
[ServiceContract(Namespace = "http://soa.demo/disponibilidad")]
public interface IServicioDisponibilidadClient
{
    [OperationContract(Action = "http://soa.demo/disponibilidad/IServicioDisponibilidad/ConsultarDisponibilidad",
                        ReplyAction = "http://soa.demo/disponibilidad/IServicioDisponibilidad/ConsultarDisponibilidadResponse")]
    DisponibilidadClienteDto ConsultarDisponibilidad(int rutaId, string fechaViaje);

    [OperationContract(Action = "http://soa.demo/disponibilidad/IServicioDisponibilidad/ReservarCupo",
                        ReplyAction = "http://soa.demo/disponibilidad/IServicioDisponibilidad/ReservarCupoResponse")]
    ReservaClienteDto ReservarCupo(string documento, int rutaId, string fechaViaje);

    [OperationContract(Action = "http://soa.demo/disponibilidad/IServicioDisponibilidad/CancelarReserva",
                        ReplyAction = "http://soa.demo/disponibilidad/IServicioDisponibilidad/CancelarReservaResponse")]
    ReservaClienteDto CancelarReserva(string documento, int rutaId, string fechaViaje);

    [OperationContract(Action = "http://soa.demo/disponibilidad/IServicioDisponibilidad/ExisteReserva",
                        ReplyAction = "http://soa.demo/disponibilidad/IServicioDisponibilidad/ExisteReservaResponse")]
    bool ExisteReserva(string documento, int rutaId, string fechaViaje);
}

[DataContract(Namespace = "http://soa.demo/disponibilidad")]
public class DisponibilidadClienteDto
{
    [DataMember] public bool Encontrada { get; set; }
    [DataMember] public int RutaId { get; set; }
    [DataMember] public string FechaViaje { get; set; } = string.Empty;
    [DataMember] public int CuposDisponibles { get; set; }
    [DataMember] public string EstadoViaje { get; set; } = string.Empty;
}

[DataContract(Namespace = "http://soa.demo/disponibilidad")]
public class ReservaClienteDto
{
    [DataMember] public bool Exito { get; set; }
    [DataMember] public string Mensaje { get; set; } = string.Empty;
    [DataMember] public int CuposRestantes { get; set; }
}