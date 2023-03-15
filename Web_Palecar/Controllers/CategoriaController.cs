using Microsoft.AspNetCore.Mvc;
using Web_Palecar.Datos;
using Web_Palecar.Models;

namespace Web_Palecar.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly AplicationDBContext _db;
        public CategoriaController(AplicationDBContext db) 
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Categoría> lista = _db.Categoria;
            return View(lista);
        }
        //GET
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Categoría categoría)
        {
            if(ModelState.IsValid)
            {
                _db.Categoria.Add(categoría);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(categoría);
           
        }
        public IActionResult Editar(int? Id)
        {
            if(Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _db.Categoria.Find(Id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Categoría categoría)
        {
            if (ModelState.IsValid)
            {
                _db.Categoria.Update(categoría);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(categoría);

        }
        public IActionResult Eliminar(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _db.Categoria.Find(Id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Categoría categoría)
        {
            if (categoría == null)
            {
                return NotFound();
            }
            _db.Categoria.Remove(categoría);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

    }
}
