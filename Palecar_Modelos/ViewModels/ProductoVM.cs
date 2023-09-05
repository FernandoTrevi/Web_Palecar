using Microsoft.AspNetCore.Mvc.Rendering;

namespace Palecar_Modelos.ViewModels
{
    public class ProductoVM
    {
        public Producto producto { get; set; }

        public IEnumerable<SelectListItem>? CategoriaLista { get; set; }

        public IEnumerable<SelectListItem>? TipoAplicacionLista { get; set; }
    }
}
