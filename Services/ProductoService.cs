using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SalesManagementAPI.Data;
using SalesManagementAPI.Models;
using System.Data;
using System.Threading.Tasks;

namespace SalesManagementAPI.Services
{
    public class ProductoService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public ProductoService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task InsertarProductoAsync(string nombre, decimal precio, decimal stock, string adicionadoPor)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_InsertarProducto", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = nombre;
                    command.Parameters.Add("@Precio", SqlDbType.Decimal).Value = precio;
                    command.Parameters.Add("@Stock", SqlDbType.Decimal).Value = stock;
                    command.Parameters.Add("@AdicionadoPor", SqlDbType.NVarChar).Value = adicionadoPor;

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task ActualizarProductoAsync(int productoID, string nombre, decimal precio, decimal stock, string modificadoPor)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_ActualizarProducto", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ProductoID", SqlDbType.Int).Value = productoID;
                    command.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = nombre;
                    command.Parameters.Add("@Precio", SqlDbType.Decimal).Value = precio;
                    command.Parameters.Add("@Stock", SqlDbType.Decimal).Value = stock;
                    command.Parameters.Add("@ModificadoPor", SqlDbType.NVarChar).Value = modificadoPor;

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task EliminarProductoAsync(int productoID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_EliminarProducto", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ProductoID", SqlDbType.Int).Value = productoID;

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Producto> ObtenerProductoAsync(int productoID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_ObtenerProducto", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ProductoID", SqlDbType.Int).Value = productoID;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Producto
                            {
                                ProductoID = reader.GetInt32(reader.GetOrdinal("ProductoID")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? null : reader.GetString(reader.GetOrdinal("Nombre")),
                                Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                                Stock = reader.GetDecimal(reader.GetOrdinal("Stock")),
                                AdicionadoPor = reader.IsDBNull(reader.GetOrdinal("AdicionadoPor")) ? null : reader.GetString(reader.GetOrdinal("AdicionadoPor"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task<List<Producto>> ObtenerProductosAsync()
        {
            var productos = new List<Producto>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_ObtenerProductos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            productos.Add(new Producto
                            {
                                ProductoID = reader.GetInt32(reader.GetOrdinal("ProductoID")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? null : reader.GetString(reader.GetOrdinal("Nombre")),
                                Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                                Stock = reader.GetDecimal(reader.GetOrdinal("Stock")),
                                AdicionadoPor = reader.IsDBNull(reader.GetOrdinal("AdicionadoPor")) ? null : reader.GetString(reader.GetOrdinal("AdicionadoPor"))
                            });
                        }
                    }
                }
            }

            return productos;
        }

    }
}