using System.ComponentModel.DataAnnotations;

namespace SalesManagementAPI.Models
{
    public class Roles
    {
        [Key]
        public int RolID { get; set; }
        public string NombreRol { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }  
    }
}
