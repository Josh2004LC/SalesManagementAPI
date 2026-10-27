using Microsoft.AspNetCore.Mvc;
using SalesManagementAPI.Models;
using SalesManagementAPI.Services;
using System.Threading.Tasks;

namespace SalesManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _productoService;

        public ProductoController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        // POST: api/producto
        [HttpPost]
        public async Task<ActionResult> InsertarProducto([FromBody] Producto request)
        {
            if (request == null || string.IsNullOrEmpty(request.Nombre) || request.Precio <= 0 || request.Stock < 0)
            {
                return BadRequest("Datos incorrectos.");
            }
            await _productoService.InsertarProductoAsync(request.Nombre, request.Precio, request.Stock, request.AdicionadoPor);
            return Ok("Producto insertado correctamente.");
        }

        // PUT: api/producto/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarProducto(int id, [FromBody] Producto request)
        {
            if (request == null || string.IsNullOrEmpty(request.Nombre) || request.Precio <= 0 || request.Stock < 0)
            {
                return BadRequest("Datos incorrectos.");
            }
            await _productoService.ActualizarProductoAsync(id, request.Nombre, request.Precio, request.Stock, request.ModificadoPor);
            return Ok("Producto actualizado correctamente.");
        }

        // DELETE: api/producto/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarProducto(int id)
        {
            await _productoService.EliminarProductoAsync(id);
            return Ok("Producto eliminado correctamente.");
        }

        // GET: api/producto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> ObtenerProductos()
        {
            var productos = await _productoService.ObtenerProductosAsync();

            if (productos == null || !productos.Any())
            {
                return NotFound("No se encontraron productos.");
            }

            return Ok(productos);
        }

        // GET: api/producto/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> ObtenerProducto(int id)
        {
            var producto = await _productoService.ObtenerProductoAsync(id);

            if (producto == null)
            {
                return NotFound($"Producto con ID {id} no encontrado.");
            }

            return Ok(producto);
        }
    }
}