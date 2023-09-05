namespace Palecar_Modelos.ViewModels
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
        public double Cantidad { get; set; } = 1;
        private double _monto;
        public double Monto
        {
            get { _monto = producto.Precio * Cantidad; return _monto; }
            set { _monto = value; }
        }
        public double Total { get; set; }
    }
}
