using Microsoft.AspNetCore.Identity;

namespace Web_Palecar.Models
{
    public class UsuariosAplicacion : IdentityUser
    {
        public string NombreCompleto { get; set; }
    }
}
