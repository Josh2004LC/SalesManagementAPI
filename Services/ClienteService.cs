using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SalesManagementAPI.Data;
using SalesManagementAPI.Models;
using System.Data;
using System.Threading.Tasks;

namespace SalesManagementAPI.Services
{
    public class ClienteService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public ClienteService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task InsertarClienteAsync(string nombre, string correo, string telefono, string pais, string provincia, string canton, string distrito, string adicionadoPor)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_InsertarCliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = nombre;
                    command.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = correo;
                    command.Parameters.Add("@Telefono", SqlDbType.NVarChar).Value = telefono ?? (object)DBNull.Value;
                    command.Parameters.Add("@Pais", SqlDbType.NVarChar).Value = pais ?? (object)DBNull.Value;
                    command.Parameters.Add("@Provincia", SqlDbType.NVarChar).Value = provincia ?? (object)DBNull.Value;
                    command.Parameters.Add("@Canton", SqlDbType.NVarChar).Value = canton ?? (object)DBNull.Value;
                    command.Parameters.Add("@Distrito", SqlDbType.NVarChar).Value = distrito ?? (object)DBNull.Value;
                    command.Parameters.Add("@AdicionadoPor", SqlDbType.NVarChar).Value = adicionadoPor;

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task ActualizarClienteAsync(int clienteID, string nombre, string correo, string telefono, string pais, string provincia, string canton, string distrito, string modificadoPor)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_ActualizarCliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ClienteID", SqlDbType.Int).Value = clienteID;
                    command.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = nombre;
                    command.Parameters.Add("@Correo", SqlDbType.NVarChar).Value = correo;
                    command.Parameters.Add("@Telefono", SqlDbType.NVarChar).Value = telefono ?? (object)DBNull.Value;
                    command.Parameters.Add("@Pais", SqlDbType.NVarChar).Value = pais ?? (object)DBNull.Value;
                    command.Parameters.Add("@Provincia", SqlDbType.NVarChar).Value = provincia ?? (object)DBNull.Value;
                    command.Parameters.Add("@Canton", SqlDbType.NVarChar).Value = canton ?? (object)DBNull.Value;
                    command.Parameters.Add("@Distrito", SqlDbType.NVarChar).Value = distrito ?? (object)DBNull.Value;
                    command.Parameters.Add("@ModificadoPor", SqlDbType.NVarChar).Value = modificadoPor;

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task EliminarClienteAsync(int clienteID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_EliminarCliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ClienteID", SqlDbType.Int).Value = clienteID;

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Cliente> ObtenerClienteAsync(int clienteID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_ObtenerCliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ClienteID", SqlDbType.Int).Value = clienteID;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Cliente
                            {
                                ClienteID = reader.GetInt32(reader.GetOrdinal("ClienteID")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? null : reader.GetString(reader.GetOrdinal("Nombre")),
                                Correo = reader.IsDBNull(reader.GetOrdinal("Correo")) ? null : reader.GetString(reader.GetOrdinal("Correo")),
                                Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                                Pais = reader.IsDBNull(reader.GetOrdinal("Pais")) ? null : reader.GetString(reader.GetOrdinal("Pais")),
                                Provincia = reader.IsDBNull(reader.GetOrdinal("Provincia")) ? null : reader.GetString(reader.GetOrdinal("Provincia")),
                                Canton = reader.IsDBNull(reader.GetOrdinal("Canton")) ? null : reader.GetString(reader.GetOrdinal("Canton")),
                                Distrito = reader.IsDBNull(reader.GetOrdinal("Distrito")) ? null : reader.GetString(reader.GetOrdinal("Distrito")),
                                AdicionadoPor = reader.IsDBNull(reader.GetOrdinal("AdicionadoPor")) ? null : reader.GetString(reader.GetOrdinal("AdicionadoPor")),
                                FechaAdicion = reader.IsDBNull(reader.GetOrdinal("FechaAdicion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaAdicion"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task<List<Cliente>> ObtenerClientesAsync()
        {
            var clientes = new List<Cliente>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_ObtenerClientes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            clientes.Add(new Cliente
                            {
                                ClienteID = reader.GetInt32(reader.GetOrdinal("ClienteID")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? null : reader.GetString(reader.GetOrdinal("Nombre")),
                                Correo = reader.IsDBNull(reader.GetOrdinal("Correo")) ? null : reader.GetString(reader.GetOrdinal("Correo")),
                                Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                                Pais = reader.IsDBNull(reader.GetOrdinal("Pais")) ? null : reader.GetString(reader.GetOrdinal("Pais")),
                                Provincia = reader.IsDBNull(reader.GetOrdinal("Provincia")) ? null : reader.GetString(reader.GetOrdinal("Provincia")),
                                Canton = reader.IsDBNull(reader.GetOrdinal("Canton")) ? null : reader.GetString(reader.GetOrdinal("Canton")),
                                Distrito = reader.IsDBNull(reader.GetOrdinal("Distrito")) ? null : reader.GetString(reader.GetOrdinal("Distrito")),
                                AdicionadoPor = reader.IsDBNull(reader.GetOrdinal("AdicionadoPor")) ? null : reader.GetString(reader.GetOrdinal("AdicionadoPor"))
                            });
                        }
                    }
                }
            }

            return clientes;
        }

        //     USO DE LINQ
        public async Task<List<Cliente>> ObtenerClientesDeCostaRicaAsync()
        {
            return await _context.Clientes
                .Where(c => c.Pais == "Costa Rica")
                .ToListAsync();
        }

        public async Task<List<Cliente>> ObtenerClientesConCorreoValidoAsync()
        {
            return await _context.Clientes
                .Where(c => c.Correo.Contains("@"))
                .ToListAsync();
        }

        public async Task<List<Cliente>> ObtenerClientesConTelefonoValidoAsync()
        {
            return await _context.Clientes
                .Where(c => c.Telefono != null && c.Telefono.Length >= 8)
                .ToListAsync();
        }
    }
}