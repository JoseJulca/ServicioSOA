using System.ServiceModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebCliente.Servicios;

namespace WebCliente.Pages;

public class IndexModel : PageModel
{
    private readonly BusESBService _busESBService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(BusESBService busESBService, ILogger<IndexModel> logger)
    {
        _busESBService = busESBService;
        _logger = logger;
    }

    [BindProperty]
    public string Documento { get; set; } = string.Empty;

    [BindProperty]
    public string Origen { get; set; } = "Lima";

    [BindProperty]
    public string Destino { get; set; } = "Cusco";

    [BindProperty]
    public string FechaViaje { get; set; } = DateTime.Today.AddDays(30).ToString("yyyy-MM-dd");

    // -----------------------------------------------------------------
    // Una sola tarjeta de resultado, cuyo titulo, contenido y boton
    // cambian segun la ultima accion ejecutada (consultar, confirmar
    // o cancelar). Evita mostrar varias tarjetas redundantes con la
    // misma informacion repetida.
    // -----------------------------------------------------------------
    public bool MostrarTarjeta { get; set; }
    public bool TarjetaExitosa { get; set; }
    public string TituloTarjeta { get; set; } = string.Empty;
    public string MensajeTarjeta { get; set; } = string.Empty;

    public string ResultadoPasajero { get; set; } = string.Empty;
    public string ResultadoEstadoPasajero { get; set; } = string.Empty;
    public string ResultadoRuta { get; set; } = string.Empty;
    public string ResultadoFecha { get; set; } = string.Empty;
    public int ResultadoCupos { get; set; }
    public string ResultadoEstadoViaje { get; set; } = string.Empty;

    // Controla cual boton de accion (si alguno) se muestra al pie de la tarjeta.
    public bool PuedeConfirmar { get; set; }
    public bool PuedeCancelar { get; set; }

    // Listas simples para los combos de Origen/Destino (coinciden con datos semilla)
    public List<string> Ciudades { get; } = new()
    {
        "Lima", "Cusco", "Arequipa", "Trujillo", "Piura", "Puno", "Tacna", "Chiclayo", "Iquitos"
    };

    public void OnGet()
    {
        // Carga inicial de la pantalla, sin consulta todavia.
    }

    public void OnPostConsultar()
    {
        MostrarTarjeta = true;
        TituloTarjeta = "Resultado";

        if (string.IsNullOrWhiteSpace(Documento))
        {
            TarjetaExitosa = false;
            MensajeTarjeta = "Debe ingresar el documento del pasajero.";
            return;
        }

        try
        {
            var resultado = _busESBService.ConsultarViaje(Documento.Trim(), Origen, Destino, FechaViaje);

            TarjetaExitosa = resultado.Exito;

            if (resultado.Exito)
            {
                ResultadoPasajero = resultado.Pasajero;
                ResultadoEstadoPasajero = resultado.EstadoPasajero;
                ResultadoRuta = resultado.Ruta;
                ResultadoFecha = resultado.FechaViaje;
                ResultadoCupos = resultado.CuposDisponibles;
                ResultadoEstadoViaje = resultado.EstadoViaje;

                if (resultado.TieneReservaPrevia)
                {
                    // El pasajero ya reservo este viaje antes: se ofrece
                    // cancelar en lugar de confirmar de nuevo.
                    PuedeConfirmar = false;
                    PuedeCancelar = true;
                    MensajeTarjeta = "Este pasajero ya tiene una reserva para este viaje.";
                }
                else
                {
                    // Solo se puede confirmar si quedo al menos 1 cupo disponible.
                    PuedeConfirmar = resultado.EstadoViaje == "Disponible";
                    PuedeCancelar = false;
                }
            }
            else
            {
                MensajeTarjeta = resultado.Mensaje;
            }
        }
        catch (EndpointNotFoundException ex)
        {
            _logger.LogError(ex, "No se pudo contactar al BusESB.");
            TarjetaExitosa = false;
            MensajeTarjeta = "No se pudo conectar con el BusESB. Verifique que el contenedor este corriendo.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al consultar el BusESB.");
            TarjetaExitosa = false;
            MensajeTarjeta = "Ocurrio un error inesperado al procesar la consulta.";
        }
    }

    public void OnPostConfirmar()
    {
        MostrarTarjeta = true;
        TituloTarjeta = "Reserva confirmada";

        if (string.IsNullOrWhiteSpace(Documento))
        {
            TarjetaExitosa = false;
            MensajeTarjeta = "Debe ingresar el documento del pasajero.";
            return;
        }

        try
        {
            var resultado = _busESBService.ConfirmarViaje(Documento.Trim(), Origen, Destino, FechaViaje);

            TarjetaExitosa = resultado.Exito;

            if (resultado.Exito)
            {
                ResultadoPasajero = resultado.Pasajero;
                ResultadoRuta = resultado.Ruta;
                ResultadoFecha = resultado.FechaViaje;
                ResultadoCupos = resultado.CuposRestantes;
                ResultadoEstadoViaje = resultado.CuposRestantes > 0 ? "Disponible" : "Agotado";
                MensajeTarjeta = resultado.Mensaje;

                // Tras confirmar, solo se ofrece cancelar esta misma reserva.
                PuedeConfirmar = false;
                PuedeCancelar = true;
            }
            else
            {
                MensajeTarjeta = resultado.Mensaje;
            }
        }
        catch (EndpointNotFoundException ex)
        {
            _logger.LogError(ex, "No se pudo contactar al BusESB.");
            TarjetaExitosa = false;
            MensajeTarjeta = "No se pudo conectar con el BusESB. Verifique que el contenedor este corriendo.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al confirmar la reserva en el BusESB.");
            TarjetaExitosa = false;
            MensajeTarjeta = "Ocurrio un error inesperado al procesar la confirmacion.";
        }
    }

    public void OnPostCancelar()
    {
        MostrarTarjeta = true;
        TituloTarjeta = "Reserva cancelada";

        if (string.IsNullOrWhiteSpace(Documento))
        {
            TarjetaExitosa = false;
            MensajeTarjeta = "Debe ingresar el documento del pasajero.";
            return;
        }

        try
        {
            var resultado = _busESBService.CancelarReserva(Documento.Trim(), Origen, Destino, FechaViaje);

            TarjetaExitosa = resultado.Exito;

            if (resultado.Exito)
            {
                ResultadoRuta = resultado.Ruta;
                ResultadoFecha = resultado.FechaViaje;
                ResultadoCupos = resultado.CuposRestantes;
                ResultadoEstadoViaje = "Disponible";
                MensajeTarjeta = resultado.Mensaje;

                // Ya se cancelo: no se ofrece ninguna accion adicional.
                // Para reservar de nuevo, el usuario debe volver a consultar.
                PuedeConfirmar = false;
                PuedeCancelar = false;
            }
            else
            {
                MensajeTarjeta = resultado.Mensaje;
            }
        }
        catch (EndpointNotFoundException ex)
        {
            _logger.LogError(ex, "No se pudo contactar al BusESB.");
            TarjetaExitosa = false;
            MensajeTarjeta = "No se pudo conectar con el BusESB. Verifique que el contenedor este corriendo.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al cancelar la reserva en el BusESB.");
            TarjetaExitosa = false;
            MensajeTarjeta = "Ocurrio un error inesperado al procesar la cancelacion.";
        }
    }
}