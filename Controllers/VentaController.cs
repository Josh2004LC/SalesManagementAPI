using Microsoft.AspNetCore.Mvc;
using SalesManagementAPI.Models;
using SalesManagementAPI.Services;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly VentaService _ventaService;

        public VentaController(VentaService ventaService)
        {
            _ventaService = ventaService;
        }

        // POST: api/venta
        [HttpPost]
        public async Task<ActionResult<int>> InsertarVenta([FromBody] VentaConDetalles ventaConDetalles)
        {
            string usuario = User.FindFirstValue(ClaimTypes.Name) ?? "sistema";

            if (ventaConDetalles == null ||
                ventaConDetalles.ClienteID <= 0 ||
                ventaConDetalles.Total <= 0 ||
                string.IsNullOrEmpty(ventaConDetalles.AdicionadoPor) ||
                ventaConDetalles.Detalles == null ||
                !ventaConDetalles.Detalles.Any())
            {
                return BadRequest("Datos de venta inválidos.");
            }

            // Inserta la venta principal y obtiene el ID correspondiente
            int ventaID = await _ventaService.InsertarVentaAsync(
                ventaConDetalles.ClienteID,
                ventaConDetalles.Total,
                ventaConDetalles.AdicionadoPor,
                usuario
            );

            // Inserta cada detalle de la venta
            foreach (var detalle in ventaConDetalles.Detalles)
            {
                await _ventaService.InsertarDetalleVentaAsync(
                    ventaID,
                    detalle.ProductoID,
                    detalle.Cantidad,
                    detalle.Subtotal,
                    ventaConDetalles.AdicionadoPor,
                    usuario
                );
            }

            return Ok(ventaID);
        }

        // GET: api/venta
        [HttpGet]
        public async Task<ActionResult<List<VentaCompleta>>> ObtenerVentas()
        {
            try
            {
                var ventas = await _ventaService.ObtenerVentasAsync();
                return Ok(ventas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener ventas: {ex.Message}");
            }
        }


        // GET: api/venta/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<VentaCompleta>> ObtenerVenta(int id)
        {
            var venta = await _ventaService.ObtenerVentaCompletaAsync(id);
            if (venta == null)
            {
                return NotFound("Venta no encontrada.");
            }
            return Ok(venta);
        }


        // PUT: api/venta/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarVenta(int id, [FromBody] VentaConDetalles ventaConDetalles)
        {
            string usuario = User.FindFirstValue(ClaimTypes.Name) ?? "sistema";

            if (ventaConDetalles == null ||
                ventaConDetalles.ClienteID <= 0 ||
                ventaConDetalles.Total <= 0 ||
                string.IsNullOrEmpty(ventaConDetalles.AdicionadoPor) ||
                ventaConDetalles.Detalles == null ||
                !ventaConDetalles.Detalles.Any())
            {
                return BadRequest("Datos de venta inválidos.");
            }

            await _ventaService.ActualizarVentaConDetallesAsync(
                id,
                ventaConDetalles.ClienteID,
                ventaConDetalles.Total,
                ventaConDetalles.AdicionadoPor,
                ventaConDetalles.Detalles,
                ventaConDetalles.AdicionadoPor,
                usuario
            );

            return Ok("Venta actualizada correctamente.");
        }

        // DELETE: api/venta/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarVenta(int id)
        {
            await _ventaService.EliminarVentaAsync(id);
            return Ok("Venta eliminada correctamente.");
        }

        // Clase para encapsular la venta con los detalles.
        public class VentaConDetalles
        {
            public int ClienteID { get; set; }
            public decimal Total { get; set; }
            public string AdicionadoPor { get; set; }
            public List<DetalleVentaItem> Detalles { get; set; }
        }


    }
}