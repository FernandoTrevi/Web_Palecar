using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palecar_AccesoADatos.Datos;
using Palecar_AccesoADatos.Datos.Repositorio.IRepositorio;
using Palecar_Modelos;
using Palecar_Utilidades;

namespace Web_Palecar.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class TipoAplicacionController : Controller
    {
        private readonly ITipoAplicacionRepositorio _tipoRepo;
        public TipoAplicacionController(ITipoAplicacionRepositorio tipoRepo)
        {
            _tipoRepo = tipoRepo;
        }
        public IActionResult Index()
        {
            IEnumerable<TipoAplicacion> lista = _tipoRepo.ObtenerTodos();
            return View(lista);
        }
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(TipoAplicacion tipoAplicacion)
        {
            if(ModelState.IsValid)
            {
                _tipoRepo.Agregar(tipoAplicacion);
                _tipoRepo.Grabar();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoAplicacion);
            
        }
        public IActionResult Editar(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _tipoRepo.Obtener(Id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(TipoAplicacion tipoAplicacion)
        {
            if (ModelState.IsValid)
            {
                _tipoRepo.Actualizar(tipoAplicacion);
                _tipoRepo.Grabar();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoAplicacion);

        }

        public IActionResult Eliminar(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _tipoRepo.Obtener(Id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(TipoAplicacion tipoAplicacion)
        {
            if (tipoAplicacion == null)
            {
                return NotFound();
            }
            _tipoRepo.Remover(tipoAplicacion);
            _tipoRepo.Grabar();
            return RedirectToAction(nameof(Index));

        }
    }
}
