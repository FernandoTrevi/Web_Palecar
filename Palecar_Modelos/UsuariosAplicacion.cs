using Microsoft.AspNetCore.Identity;

namespace Palecar_Modelos
{
    public class UsuariosAplicacion : IdentityUser
    {
        public string NombreCompleto { get; set; }
    }
}
