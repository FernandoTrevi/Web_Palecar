using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Palecar_Modelos;


namespace Palecar_AccesoADatos.Datos
{
    public class AplicationDBContext:IdentityDbContext
    {
        public AplicationDBContext(DbContextOptions<AplicationDBContext>options):base(options)
        {
            
        }
        public DbSet<Categoría> Categoria { get; set; }
        public DbSet<TipoAplicacion> TipoAplicacions { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<UsuariosAplicacion> UsuariosAplicacions { get;set; }
    }
}
