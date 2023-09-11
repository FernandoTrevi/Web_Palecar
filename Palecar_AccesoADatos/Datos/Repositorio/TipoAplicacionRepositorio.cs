using Palecar_AccesoADatos.Datos.Repositorio.IRepositorio;
using Palecar_Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palecar_AccesoADatos.Datos.Repositorio
{
    public class TipoAplicacionRepositorio : Repositorio<TipoAplicacion>, ITipoAplicacionRepositorio
    {
        private readonly AplicationDBContext _db;
        public TipoAplicacionRepositorio(AplicationDBContext db):base(db) 
        {
            _db = db;
        }
        public void Actualizar(TipoAplicacion tipoAplicacion)
        {
            var antTipoAplicacion = _db.TipoAplicacions.FirstOrDefault(t => t.Id == tipoAplicacion.Id);
            if(antTipoAplicacion != null)
            {
                antTipoAplicacion.Nombre = tipoAplicacion.Nombre;
            }
        }
    }
}
