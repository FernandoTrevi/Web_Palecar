using Palecar_AccesoADatos.Datos.Repositorio.IRepositorio;
using Palecar_Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palecar_AccesoADatos.Datos.Repositorio
{
    public class CategoriaRepositorio : Repositorio<Categoría>, ICategoriaRepositorio
    {
        private readonly AplicationDBContext _db;
        public CategoriaRepositorio(AplicationDBContext db): base(db) 
        {
            _db = db;
        }
        public void Actualizar(Categoría categoria)
        {
            var catAnterior = _db.Categoria.FirstOrDefault(c => c.Id == categoria.Id);
            if (catAnterior != null)
            {
                catAnterior.NombreCategoria = categoria.NombreCategoria;
                catAnterior.MostrarOrden = categoria.MostrarOrden;  
            }
        }
    }
}
