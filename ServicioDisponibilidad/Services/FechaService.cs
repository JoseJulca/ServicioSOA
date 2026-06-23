using System.Globalization;

namespace ServicioDisponibilidad.Services
{
    public class FechaService : IFechaService
    {
        public bool TryParseFecha(string fechaViaje, out DateTime fecha)
        {
            return DateTime.TryParse(fechaViaje,CultureInfo.InvariantCulture,DateTimeStyles.None,out fecha);
        }
    }
}
