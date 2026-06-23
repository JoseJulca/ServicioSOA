using System.Data;
using Microsoft.Data.SqlClient;

namespace ServicioPasajeros.Repositories
{
    public class PasajeroRepository : IPasajeroRepository
    {
        private readonly string _connectionString;

        public PasajeroRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SoaDemoDb")
                ?? throw new InvalidOperationException("Cadena de conexión 'SoaDemoDb' no configurada.");
        }

        public PasajeroDto ValidarPasajero(string documento)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("dbo.sp_ValidarPasajero", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Documento", SqlDbType.VarChar, 20).Value = documento;

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new PasajeroDto
                {
                    Encontrado = true,
                    Documento = reader.GetString(0),
                    NombreCompleto = reader.GetString(1),
                    Estado = reader.GetString(2)
                };
            }

            return new PasajeroDto
            {
                Encontrado = false,
                Documento = documento,
                NombreCompleto = string.Empty,
                Estado = "NoEncontrado"
            };
        }
    }
}
