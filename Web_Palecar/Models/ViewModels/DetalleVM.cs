namespace Web_Palecar.Models.ViewModels
{
    public class DetalleVM
    {
        public DetalleVM()
        {
            producto = new Producto();
        }
        public Producto producto { get; set; }
        public bool ExisteEnCarro  { get; set; }
        public IEnumerable<Producto>? productosLista { get; set; }

    }
}
