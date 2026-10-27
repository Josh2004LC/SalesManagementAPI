using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SalesManagementAPI.Models;
using SalesManagementAPI.Services;

namespace SalesManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // POST: api/usuario
        [HttpPost]
        public async Task<ActionResult> InsertarUsuario([FromBody] Usuario request)
        {
            if (request == null ||
                string.IsNullOrEmpty(request.NombreUsuario) ||
                string.IsNullOrEmpty(request.Contraseña) ||
                string.IsNullOrEmpty(request.NombreCompleto) ||
                string.IsNullOrEmpty(request.Correo) ||
                string.IsNullOrEmpty(request.RolNombre))
            {
                return BadRequest("Faltan datos obligatorios en la solicitud.");
            }

            try
            {
                await _usuarioService.InsertarUsuarioAsync(
                    request.NombreUsuario,
                    request.Contraseña,
                    request.NombreCompleto,
                    request.Correo,
                    request.RolNombre
                );

                return Ok("Usuario insertado correctamente.");
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Error al insertar usuario: {ex.Message}");
            }
        }
        // GET: api/usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> ObtenerUsuarios()
        {
            var usuarios = await _usuarioService.ObtenerUsuariosAsync();

            if (usuarios == null || !usuarios.Any())
            {
                return NotFound("No se encontraron usuarios.");
            }

            return Ok(usuarios);
        }

        // GET: api/cliente/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> ObtenerUsuario(int id)
        {
            var cliente = await _usuarioService.ObtenerUsuarioAsync(id);
            if (cliente == null)
            {
                return NotFound($"Usuario con ID {id} no encontrado.");
            }
            return Ok(cliente);
        }
    }
}
