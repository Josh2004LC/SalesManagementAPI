using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SalesManagementAPI.Controllers;
using SalesManagementAPI.Data;
using SalesManagementAPI.Models;
using System.Threading.Tasks;

namespace SalesManagementAPI.Services
{
    public class LoginService
    {
        private readonly ApplicationDbContext _context;

        public LoginService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> LoginAsync(string nombreUsuario, string contraseña)
        {
            var usuarioIDParam = new SqlParameter("@UsuarioID", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
            var nombreCompletoParam = new SqlParameter("@NombreCompleto", System.Data.SqlDbType.NVarChar, 100) { Direction = System.Data.ParameterDirection.Output };
            var rolIDParam = new SqlParameter("@RolID", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
            var rolNombreParam = new SqlParameter("@RolNombre", System.Data.SqlDbType.NVarChar, 50) { Direction = System.Data.ParameterDirection.Output };
            var estadoParam = new SqlParameter("@Estado", System.Data.SqlDbType.NVarChar, 20) { Direction = System.Data.ParameterDirection.Output };

            // Ejecutar el procedimiento almacenado de login
            var result = await _context.Database.ExecuteSqlRawAsync("EXEC sp_LoginUsuario @NombreUsuario, @Contraseña, @UsuarioID OUTPUT, @NombreCompleto OUTPUT, @RolID OUTPUT, @RolNombre OUTPUT, @Estado OUTPUT",
                new SqlParameter("@NombreUsuario", nombreUsuario),
                new SqlParameter("@Contraseña", contraseña),
                usuarioIDParam,
                nombreCompletoParam,
                rolIDParam,
                rolNombreParam,
                estadoParam
            );


            // Si el procedimiento devuelve un ID de usuario, el login fue exitoso
            if (usuarioIDParam.Value != DBNull.Value)
            {
                return new Usuario
                {
                    UsuarioID = (int)usuarioIDParam.Value,
                    NombreCompleto = (string)nombreCompletoParam.Value,
                    RolNombre = (string)rolNombreParam.Value,
                    RolID = (int)rolIDParam.Value,
                    Estado = (string)estadoParam.Value
                };
            }

            return null; 
        }
    }
}
