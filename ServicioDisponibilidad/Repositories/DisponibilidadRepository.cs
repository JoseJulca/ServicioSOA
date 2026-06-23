using System.Data;
using Microsoft.Data.SqlClient;

namespace ServicioDisponibilidad.Repositories
{
    public class DisponibilidadRepository : IDisponibilidadRepository
    {
        private readonly string _connectionString;

        public DisponibilidadRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SoaDemoDb")
                ?? throw new InvalidOperationException("Cadena de conexión 'SoaDemoDb' no configurada.");
        }

        public DisponibilidadDto ConsultarDisponibilidad(int rutaId, DateTime fechaViaje, string fechaOriginal)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("dbo.sp_ConsultarDisponibilidad", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@RutaId", SqlDbType.Int).Value = rutaId;
            command.Parameters.Add("@FechaViaje", SqlDbType.Date).Value = fechaViaje.Date;

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                var cupos = reader.GetInt32(2);

                return new DisponibilidadDto
                {
                    Encontrada = true,
                    RutaId = reader.GetInt32(0),
                    FechaViaje = reader.GetDateTime(1).ToString("yyyy-MM-dd"),
                    CuposDisponibles = cupos,
                    EstadoViaje = cupos > 0 ? "Disponible" : "Agotado"
                };
            }

            return new DisponibilidadDto
            {
                Encontrada = false,
                RutaId = rutaId,
                FechaViaje = fechaOriginal,
                CuposDisponibles = 0,
                EstadoViaje = "SinRegistro"
            };
        }
    }
}
