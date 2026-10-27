using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SalesManagementAPI.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public int RolID { get; set; }  
        public string Estado { get; set; }
        public string RolNombre { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
