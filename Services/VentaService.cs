using Microsoft.Data.SqlClient;
using SalesManagementAPI.Data;
using SalesManagementAPI.Models;
using System.Data;
using SalesManagementAPI.Controllers;

namespace SalesManagementAPI.Services
{
    public class VentaService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public VentaService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        // Inserta la venta y retorna el ID correspondiente.
        public async Task<int> InsertarVentaAsync(int clienteID, decimal total, string adicionadoPor, string usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_InsertarVenta", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ClienteID", SqlDbType.Int).Value = clienteID;
                    command.Parameters.Add("@Total", SqlDbType.Decimal).Value = total;
                    command.Parameters.Add("@AdicionadoPor", SqlDbType.NVarChar, 50).Value = adicionadoPor;
                    command.Parameters.Add("@Usuario", SqlDbType.NVarChar, 50).Value = usuario;

                    var ventaIDParam = new SqlParameter("@VentaID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(ventaIDParam);

                    await command.ExecuteNonQueryAsync();
                    return (int)ventaIDParam.Value;
                }
            }
        }

        // Inserta un detalle de la venta.
        public async Task InsertarDetalleVentaAsync(int ventaID, int productoID, decimal cantidad, decimal subtotal, string adicionadoPor, string usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_InsertarDetalleVenta", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@VentaID", SqlDbType.Int).Value = ventaID;
                    command.Parameters.Add("@ProductoID", SqlDbType.Int).Value = productoID;
                    command.Parameters.Add("@Cantidad", SqlDbType.Decimal).Value = cantidad;
                    command.Parameters.Add("@Subtotal", SqlDbType.Decimal).Value = subtotal;
                    command.Parameters.Add("@AdicionadoPor", SqlDbType.NVarChar, 50).Value = adicionadoPor;
                    command.Parameters.Add("@Usuario", SqlDbType.NVarChar, 50).Value = usuario;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Obtiene la venta por el ID.
        public async Task<VentaCompleta> ObtenerVentaCompletaAsync(int ventaID)
        {
            VentaCompleta venta = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_ObtenerVenta", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@VentaID", SqlDbType.Int).Value = ventaID;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Primera fila: Esta es la cabecera de la venta
                            if (venta == null)
                            {
                                venta = new VentaCompleta
                                {
                                    VentaID = reader.GetInt32(reader.GetOrdinal("VentaID")),
                                    ClienteID = reader.GetInt32(reader.GetOrdinal("ClienteID")),
                                    Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                    Total = reader.GetDecimal(reader.GetOrdinal("Total")),
                                    AdicionadoPor = reader.IsDBNull(reader.GetOrdinal("AdicionadoPor")) ? null : reader.GetString(reader.GetOrdinal("AdicionadoPor")),
                                    FechaAdicion = reader.GetDateTime(reader.GetOrdinal("FechaAdicion")),
                                    ModificadoPor = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? null : reader.GetString(reader.GetOrdinal("ModificadoPor")),
                                    FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModificacion"))
                                };
                            }

                            // Muestra el detalle si existe
                            int detalleID = reader.IsDBNull(reader.GetOrdinal("DetalleID")) ? 0 : reader.GetInt32(reader.GetOrdinal("DetalleID"));
                            if (detalleID > 0)
                            {
                                var detalle = new DetalleVenta
                                {
                                    DetalleID = detalleID,
                                    VentaID = reader.GetInt32(reader.GetOrdinal("VentaID")),
                                    ProductoID = reader.GetInt32(reader.GetOrdinal("ProductoID")),
                                    NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
                                    Cantidad = reader.GetDecimal(reader.GetOrdinal("Cantidad")),
                                    Subtotal = reader.GetDecimal(reader.GetOrdinal("Subtotal")),
                                    AdicionadoPor = reader.IsDBNull(reader.GetOrdinal("DetalleAdicionadoPor")) ? null : reader.GetString(reader.GetOrdinal("DetalleAdicionadoPor")),
                                    FechaAdicion = reader.GetDateTime(reader.GetOrdinal("DetalleFechaAdicion")),
                                    ModificadoPor = reader.IsDBNull(reader.GetOrdinal("DetalleModificadoPor")) ? null : reader.GetString(reader.GetOrdinal("DetalleModificadoPor")),
                                    FechaModificacion = reader.IsDBNull(reader.GetOrdinal("DetalleFechaModificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DetalleFechaModificacion"))
                                };

                                venta.Detalles.Add(detalle);
                            }
                        }
                    }
                }
            }
            return venta;
        }


        // Actualiza los datos principales de la venta.
        public async Task ActualizarVentaAsync(int ventaID, int clienteID, decimal total, string modificadoPor)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_ActualizarVenta", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@VentaID", SqlDbType.Int).Value = ventaID;
                    command.Parameters.Add("@ClienteID", SqlDbType.Int).Value = clienteID;
                    command.Parameters.Add("@Total", SqlDbType.Decimal).Value = total;
                    command.Parameters.Add("@ModificadoPor", SqlDbType.NVarChar, 50).Value = modificadoPor;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Elimina la venta.
        public async Task EliminarVentaAsync(int ventaID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_EliminarVenta", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@VentaID", SqlDbType.Int).Value = ventaID;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Actualiza los detalles de una venta: primero se eliminan los detalles actuales usando el SP y luego inserta los nuevos.
        public async Task ActualizarDetallesVentaAsync(int ventaID, List<Models.DetalleVentaItem> detalles, string adicionadoPor, string usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Se llama al SP para eliminar los detalles existentes
                using (var deleteCommand = new SqlCommand("sp_EliminarDetallesVentaPorVenta", connection))
                {
                    deleteCommand.CommandType = CommandType.StoredProcedure;
                    deleteCommand.Parameters.Add("@VentaID", SqlDbType.Int).Value = ventaID;
                    await deleteCommand.ExecuteNonQueryAsync();
                }

                // Inserta los detalles nuevos 
                foreach (var detalle in detalles)
                {
                    using (var insertCommand = new SqlCommand("sp_InsertarDetalleVenta", connection))
                    {
                        insertCommand.CommandType = CommandType.StoredProcedure;
                        insertCommand.Parameters.Add("@VentaID", SqlDbType.Int).Value = ventaID;
                        insertCommand.Parameters.Add("@ProductoID", SqlDbType.Int).Value = detalle.ProductoID;
                        insertCommand.Parameters.Add("@Cantidad", SqlDbType.Decimal).Value = detalle.Cantidad;
                        insertCommand.Parameters.Add("@Subtotal", SqlDbType.Decimal).Value = detalle.Subtotal;
                        insertCommand.Parameters.Add("@AdicionadoPor", SqlDbType.NVarChar, 50).Value = adicionadoPor;
                        insertCommand.Parameters.Add("@Usuario", SqlDbType.NVarChar, 50).Value = usuario;
                        await insertCommand.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        // Actualiza la venta principal con los detalles.
        public async Task ActualizarVentaConDetallesAsync(int ventaID, int clienteID, decimal total, string modificadoPor, List<Models.DetalleVentaItem> detalles, string adicionadoPor, string usuario)
        {
            await ActualizarVentaAsync(ventaID, clienteID, total, modificadoPor);
            await ActualizarDetallesVentaAsync(ventaID, detalles, adicionadoPor, usuario);
        }

        // Obtiene todas las ventas
        public async Task<List<VentaCompleta>> ObtenerVentasAsync()
        {
            List<VentaCompleta> ventas = new List<VentaCompleta>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_ObtenerVentas", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var venta = new VentaCompleta
                            {
                                VentaID = reader.GetInt32(reader.GetOrdinal("VentaID")),
                                ClienteID = reader.GetInt32(reader.GetOrdinal("ClienteID")),
                                Nombre= reader.GetString(reader.GetOrdinal("Nombre")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                Total = reader.GetDecimal(reader.GetOrdinal("Total")),
                                AdicionadoPor = reader.IsDBNull(reader.GetOrdinal("AdicionadoPor")) ? null : reader.GetString(reader.GetOrdinal("AdicionadoPor")),
                                FechaAdicion = reader.GetDateTime(reader.GetOrdinal("FechaAdicion")),
                                ModificadoPor = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? null : reader.GetString(reader.GetOrdinal("ModificadoPor")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                Detalles = new List<DetalleVenta>()
                            };

                            ventas.Add(venta);
                        }
                    }
                }

                // Opcionalmente, puedes cargar los detalles de cada venta
                // Esto puede ser ineficiente si hay muchas ventas, considera si realmente necesitas los detalles
                foreach (var venta in ventas)
                {
                    var ventaCompleta = await ObtenerVentaCompletaAsync(venta.VentaID);
                    if (ventaCompleta != null && ventaCompleta.Detalles.Any())
                    {
                        venta.Detalles = ventaCompleta.Detalles;
                    }
                }
            }

            return ventas;
        }
        
    }
}