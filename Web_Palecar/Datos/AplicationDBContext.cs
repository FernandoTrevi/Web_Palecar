using Microsoft.EntityFrameworkCore;
using Web_Palecar.Models;

namespace Web_Palecar.Datos
{
    public class AplicationDBContext: DbContext
    {
        public AplicationDBContext(DbContextOptions<AplicationDBContext>options):base(options)
        {
            
        }
        public DbSet<Categoría> Categoria { get; set; }
        public DbSet<TipoAplicacion> TipoAplicacions { get; set; }
        public DbSet<Producto> Productos { get; set; }
    }
}
