using Microsoft.AspNetCore.Mvc.Rendering;
using Palecar_AccesoADatos.Datos.Repositorio.IRepositorio;
using Palecar_Modelos;
using Palecar_Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Palecar_AccesoADatos.Datos.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly AplicationDBContext _db;
        public ProductoRepositorio(AplicationDBContext db) : base(db) 
        {
            _db = db;
        }
        public void Actualizar(Producto producto)
        {
            _db.Update(producto);
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropDownList(string obj)
        {
            if (obj == WC.CategoriaNombre)
            {
                return _db.Categoria.Select(c => new SelectListItem
                {
                    Text = c.NombreCategoria,
                    Value = c.Id.ToString()
                });
            }
            if (obj == WC.TipoAplcacionNombre)
            {
                return _db.TipoAplicacions.Select(t => new SelectListItem
                {
                    Text = t.Nombre,
                    Value = t.Id.ToString()
                });
            }

            return null;
        }
    }
}
