using Microsoft.AspNetCore.Mvc;
using Web_Palecar.Datos;
using Web_Palecar.Models;

namespace Web_Palecar.Controllers
{
    public class TipoAplicacionController : Controller
    {
        private readonly AplicationDBContext _db;
        public TipoAplicacionController(AplicationDBContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<TipoAplicacion> lista = _db.TipoAplicacions;
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
                _db.TipoAplicacions.Add(tipoAplicacion);
                _db.SaveChanges();
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
            var obj = _db.TipoAplicacions.Find(Id);
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
                _db.TipoAplicacions.Update(tipoAplicacion);
                _db.SaveChanges();
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
            var obj = _db.TipoAplicacions.Find(Id);
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
            _db.TipoAplicacions.Remove(tipoAplicacion);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
