namespace Web_Palecar.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Producto>? productos  { get; set; }
        public IEnumerable<Categoría>? categorías { get; set; }
    }
}
