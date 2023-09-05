using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Palecar_Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Ingrese el nombre del producto")]
        public string NombreProducto { get; set; }

        [Required(ErrorMessage ="Ingrese el precio del producto")]
        [Range(1, double.MaxValue)]
        public double Precio { get; set; }

        public string? UrlImagen { get; set; }

        public int IdCategoria { get; set; }
        [ForeignKey("IdCategoria")]
        public virtual Categoría? Categoría { get; set; }

        public int IdTipoAplicacion { get; set; }
        [ForeignKey("IdTipoAplicacion")]
        public virtual TipoAplicacion? TipoAplicacion { get; set; }
    }
}
