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
    public class UsuarioAplicacionRepositorio : Repositorio<UsuariosAplicacion>, IUsuarioAplicacionRepositorio
    {
        private readonly AplicationDBContext _db;
        public UsuarioAplicacionRepositorio(AplicationDBContext db) : base(db) 
        {
            _db = db;
        }
        public void Actualizar(UsuariosAplicacion usuariosAplicacion)
        {
            _db.Update(usuariosAplicacion);
        }

    }
}
