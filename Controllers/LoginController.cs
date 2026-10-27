using Microsoft.AspNetCore.Mvc;
using SalesManagementAPI.Models;
using SalesManagementAPI.Services;
using System.Threading.Tasks;

namespace SalesManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        // POST: api/login
        [HttpPost]
        public async Task<ActionResult<Usuario>> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.NombreUsuario) || string.IsNullOrEmpty(request.Contraseña))
            {
                return BadRequest("Nombre de usuario o contraseña no proporcionados.");
            }

            var usuario = await _loginService.LoginAsync(request.NombreUsuario, request.Contraseña);

            if (usuario == null)
            {
                return Unauthorized("Credenciales incorrectas.");
            }

            return Ok(usuario); // Devuelve los detalles del usuario si el login fue exitoso
        }
    }

    public class LoginRequest
    {
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
    }

}
