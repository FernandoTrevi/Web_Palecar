﻿namespace Web_Palecar.Models.ViewModels
{
    public class ProductoUsuarioVM
    {
        public ProductoUsuarioVM()
        {
            productoLista = new List<Producto>();   
        }
        public UsuariosAplicacion usuariosAplicacion { get; set; }
        public IEnumerable<Producto> productoLista { get; set; }
    }
}