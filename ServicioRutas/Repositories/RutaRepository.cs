using System.Data;
using Microsoft.Data.SqlClient;

namespace ServicioRutas.Repositories
{
    public class RutaRepository : IRutaRepository
    {
        private readonly string _connectionString;

        public RutaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SoaDemoDb")
                ?? throw new InvalidOperationException("Cadena de conexión 'SoaDemoDb' no configurada.");
        }

        public RutaDto ObtenerRuta(string origen, string destino)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("dbo.sp_ObtenerRuta", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Origen", SqlDbType.VarChar, 50).Value = origen;
            command.Parameters.Add("@Destino", SqlDbType.VarChar, 50).Value = destino;

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new RutaDto
                {
                    Encontrada = true,
                    RutaId = reader.GetInt32(0),
                    Origen = reader.GetString(1),
                    Destino = reader.GetString(2),
                    Estado = reader.GetString(3)
                };
            }

            return new RutaDto
            {
                Encontrada = false,
                RutaId = 0,
                Origen = origen,
                Destino = destino,
                Estado = "NoEncontrada"
            };
        }
    }
}
