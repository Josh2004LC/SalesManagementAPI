using Microsoft.AspNetCore.Mvc;
using SalesManagementAPI.Models;
using SalesManagementAPI.Services;
using System.Threading.Tasks;

namespace SalesManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;
        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // POST: api/cliente
        [HttpPost]
        public async Task<ActionResult> InsertarCliente([FromBody] Cliente request)
        {
            if (request == null || string.IsNullOrEmpty(request.Nombre) || string.IsNullOrEmpty(request.Correo))
            {
                return BadRequest("Faltan datos en la solicitud.");
            }
            await _clienteService.InsertarClienteAsync(request.Nombre, request.Correo, request.Telefono, request.Pais, request.Provincia, request.Canton, request.Distrito, request.AdicionadoPor);
            return Ok("Cliente insertado correctamente.");
        }

        // PUT: api/cliente/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarCliente(int id, [FromBody] Cliente request)
        {
            if (request == null || string.IsNullOrEmpty(request.Nombre) || string.IsNullOrEmpty(request.Correo))
            {
                return BadRequest("Faltan datos en la solicitud.");
            }
            await _clienteService.ActualizarClienteAsync(id, request.Nombre, request.Correo, request.Telefono, request.Pais, request.Provincia, request.Canton, request.Distrito, request.ModificadoPor);
            return Ok("Cliente actualizado correctamente.");
        }

        // DELETE: api/cliente/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarCliente(int id)
        {
            await _clienteService.EliminarClienteAsync(id);
            return Ok("Cliente eliminado correctamente.");
        }

        // GET: api/cliente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> ObtenerClientes()
        {
            var clientes = await _clienteService.ObtenerClientesAsync();

            if (clientes == null || !clientes.Any())
            {
                return NotFound("No se encontraron clientes.");
            }

            return Ok(clientes);
        }

        // GET: api/cliente/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> ObtenerCliente(int id)
        {
            var cliente = await _clienteService.ObtenerClienteAsync(id);
            if (cliente == null)
            {
                return NotFound($"Cliente con ID {id} no encontrado.");
            }
            return Ok(cliente);
        }

        // USO DE LINQ
        [HttpGet("CostaRica")]
        public async Task<ActionResult<List<Cliente>>> GetClientesDeCostaRica()
        {
            var clientes = await _clienteService.ObtenerClientesDeCostaRicaAsync();
            return Ok(clientes);
        }

        [HttpGet("CorreoValido")]
        public async Task<ActionResult<List<Cliente>>> GetClientesConCorreo()
        {
            var clientes = await _clienteService.ObtenerClientesConCorreoValidoAsync();
            return Ok(clientes);
        }

        [HttpGet("TelefonoValido")]
        public async Task<ActionResult<List<Cliente>>> GetClientesConTelefonoValido()
        {
            var clientes = await _clienteService.ObtenerClientesConTelefonoValidoAsync();
            return Ok(clientes);
        }
    }
}