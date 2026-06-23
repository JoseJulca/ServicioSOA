using System.Data;
using Microsoft.Data.SqlClient;

namespace ServicioDisponibilidad.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly string _connectionString;

        public ReservaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SoaDemoDb")
                ?? throw new InvalidOperationException("Cadena de conexión 'SoaDemoDb' no configurada.");
        }

        public ReservaDto ReservarCupo(string documento, int rutaId, DateTime fechaViaje)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("dbo.sp_ReservarCupo", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Documento", SqlDbType.VarChar, 20).Value = documento;
            command.Parameters.Add("@RutaId", SqlDbType.Int).Value = rutaId;
            command.Parameters.Add("@FechaViaje", SqlDbType.Date).Value = fechaViaje.Date;

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new ReservaDto
                {
                    Exito = reader.GetBoolean(0),
                    Mensaje = reader.GetString(1),
                    CuposRestantes = reader.GetInt32(2)
                };
            }

            return new ReservaDto
            {
                Exito = false,
                Mensaje = "El Stored Procedure no devolvió resultado.",
                CuposRestantes = 0
            };
        }

        public ReservaDto CancelarReserva(string documento, int rutaId, DateTime fechaViaje)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("dbo.sp_CancelarReserva", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Documento", SqlDbType.VarChar, 20).Value = documento;
            command.Parameters.Add("@RutaId", SqlDbType.Int).Value = rutaId;
            command.Parameters.Add("@FechaViaje", SqlDbType.Date).Value = fechaViaje.Date;

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new ReservaDto
                {
                    Exito = reader.GetBoolean(0),
                    Mensaje = reader.GetString(1),
                    CuposRestantes = reader.GetInt32(2)
                };
            }

            return new ReservaDto
            {
                Exito = false,
                Mensaje = "El Stored Procedure no devolvió resultado.",
                CuposRestantes = 0
            };
        }

        public bool ExisteReserva(string documento, int rutaId, DateTime fechaViaje)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("dbo.sp_ExisteReserva", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Documento", SqlDbType.VarChar, 20).Value = documento;
            command.Parameters.Add("@RutaId", SqlDbType.Int).Value = rutaId;
            command.Parameters.Add("@FechaViaje", SqlDbType.Date).Value = fechaViaje.Date;

            var resultado = command.ExecuteScalar();
            return resultado is int valor && valor == 1;
        }
    }
}
