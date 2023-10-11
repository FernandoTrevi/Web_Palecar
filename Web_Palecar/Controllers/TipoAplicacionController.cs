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
                TempData[WC.Exitosa] = "Tipo de aplicación creado exitosamente";

                return RedirectToAction(nameof(Index));
            }
            TempData[WC.Error] = "Error al crear un nuevo tipo de aplicación";

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
                TempData[WC.Exitosa] = "Tipo de aplicación editado exitosamente";

                return RedirectToAction(nameof(Index));
            }
            TempData[WC.Error] = "No se pudo editar el tipo de aplicación";

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
                TempData[WC.Error] = "No se pudo eliminar el tipo de aplicación";

                return NotFound();
            }
            _tipoRepo.Remover(tipoAplicacion);
            _tipoRepo.Grabar();
            TempData[WC.Exitosa] = "Se eliminó exitosamente";

            return RedirectToAction(nameof(Index));

        }
    }
}
