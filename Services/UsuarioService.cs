using Microsoft.Data.SqlClient;
using SalesManagementAPI.Data;
using SalesManagementAPI.Models;
using System.Data;

namespace SalesManagementAPI.Services
{
    public class UsuarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public UsuarioService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task InsertarUsuarioAsync(string nombreUsuario, string contraseña, string nombreCompleto, string correo, string rolNombre)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_RegistrarUsuario", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@NombreUsuario", SqlDbType.NVarChar, 50).Value = nombreUsuario;
                    command.Parameters.Add("@Contraseña", SqlDbType.NVarChar, 50).Value = contraseña;
                    command.Parameters.Add("@NombreCompleto", SqlDbType.NVarChar, 100).Value = nombreCompleto;
                    command.Parameters.Add("@Correo", SqlDbType.NVarChar, 100).Value = correo;
                    command.Parameters.Add("@RolNombre", SqlDbType.NVarChar, 50).Value = rolNombre;

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Usuario> ObtenerUsuarioAsync(int usuarioID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_ObtenerUsuario", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = usuarioID;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Usuario
                            {
                                UsuarioID = reader.GetInt32(reader.GetOrdinal("UsuarioID")),
                                NombreUsuario = reader.IsDBNull(reader.GetOrdinal("NombreUsuario")) ? null : reader.GetString(reader.GetOrdinal("NombreUsuario")),
                                NombreCompleto = reader.IsDBNull(reader.GetOrdinal("NombreCompleto")) ? null : reader.GetString(reader.GetOrdinal("NombreCompleto")),
                                Correo = reader.IsDBNull(reader.GetOrdinal("Correo")) ? null : reader.GetString(reader.GetOrdinal("Correo")),
                                RolNombre = reader.IsDBNull(reader.GetOrdinal("NombreRol")) ? null : reader.GetString(reader.GetOrdinal("NombreRol")),
                                FechaRegistro = reader.IsDBNull(reader.GetOrdinal("FechaRegistro")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                                Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? null : reader.GetString(reader.GetOrdinal("Estado"))
                            };
                        }
                    }
                }
            }
            return null;
        }


        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            var listaUsuarios = new List<Usuario>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_ObtenerUsuarios", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var usuario = new Usuario
                            {
                                UsuarioID = reader.GetInt32(reader.GetOrdinal("UsuarioID")),
                                NombreUsuario = reader.IsDBNull(reader.GetOrdinal("NombreUsuario")) ? null : reader.GetString(reader.GetOrdinal("NombreUsuario")),
                                NombreCompleto = reader.IsDBNull(reader.GetOrdinal("NombreCompleto")) ? null : reader.GetString(reader.GetOrdinal("NombreCompleto")),
                                Correo = reader.IsDBNull(reader.GetOrdinal("Correo")) ? null : reader.GetString(reader.GetOrdinal("Correo")),
                                RolNombre = reader.IsDBNull(reader.GetOrdinal("NombreRol")) ? null : reader.GetString(reader.GetOrdinal("NombreRol")),
                                Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? null : reader.GetString(reader.GetOrdinal("Estado"))
                            };

                            listaUsuarios.Add(usuario);
                        }
                    }
                }
            }

            return listaUsuarios;
        }

    }
}