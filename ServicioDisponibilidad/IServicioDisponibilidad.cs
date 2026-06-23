using System.Runtime.Serialization;
using CoreWCF;

namespace ServicioDisponibilidad;

[ServiceContract(Namespace = "http://soa.demo/disponibilidad")]
public interface IServicioDisponibilidad
{
    [OperationContract]
    DisponibilidadDto ConsultarDisponibilidad(int rutaId, string fechaViaje);


    [OperationContract]
    ReservaDto ReservarCupo(string documento, int rutaId, string fechaViaje);

    [OperationContract]
    ReservaDto CancelarReserva(string documento, int rutaId, string fechaViaje);

    [OperationContract]
    bool ExisteReserva(string documento, int rutaId, string fechaViaje);
}

[DataContract(Namespace = "http://soa.demo/disponibilidad")]
public class DisponibilidadDto
{
    [DataMember] public bool Encontrada { get; set; }
    [DataMember] public int RutaId { get; set; }
    [DataMember] public string FechaViaje { get; set; } = string.Empty;
    [DataMember] public int CuposDisponibles { get; set; }
    [DataMember] public string EstadoViaje { get; set; } = string.Empty; // Disponible / Agotado
}

[DataContract(Namespace = "http://soa.demo/disponibilidad")]
public class ReservaDto
{
    [DataMember] public bool Exito { get; set; }
    [DataMember] public string Mensaje { get; set; } = string.Empty;
    [DataMember] public int CuposRestantes { get; set; }
}
