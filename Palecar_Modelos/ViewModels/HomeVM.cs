namespace Palecar_Modelos.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Producto>? productos  { get; set; }
        public IEnumerable<Categoría>? categorías { get; set; }
    }
}
