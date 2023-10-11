using Microsoft.AspNetCore.Mvc;
using Web_Palecar;
using Palecar_Modelos;
using Palecar_AccesoADatos.Datos;
using Microsoft.AspNetCore.Authorization;
using Palecar_Utilidades;
using System.Data;
using Palecar_AccesoADatos.Datos.Repositorio;
using Palecar_AccesoADatos.Datos.Repositorio.IRepositorio;

namespace Web_Palecar.Controllers
{
    [Authorize(Roles = WC.AdminRole)]

    public class CategoriaController : Controller
    {
        private readonly ICategoriaRepositorio _catRepo;
        public CategoriaController(ICategoriaRepositorio catRepo) 
        {
            _catRepo = catRepo;
        }
        public IActionResult Index()
        {
            IEnumerable<Categoría> lista = _catRepo.ObtenerTodos();
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
                _catRepo.Agregar(categoría);
                _catRepo.Grabar();
                TempData[WC.Exitosa] = "Categoría creada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            TempData[WC.Error] = "Error al crear nueva categoría";

            return View(categoría);
           
        }
        public IActionResult Editar(int? Id)
        {
            if(Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _catRepo.Obtener(Id.GetValueOrDefault());
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
                _catRepo.Actualizar(categoría);
                _catRepo.Grabar();
                TempData[WC.Exitosa] = "Categoría editada exitosamente";

                return RedirectToAction(nameof(Index));
            }
            TempData[WC.Exitosa] = "Error al editar categoría";

            return View(categoría);

        }
        public IActionResult Eliminar(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _catRepo.Obtener(Id.GetValueOrDefault());
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
                TempData[WC.Error] = "Error al eliminar la categoría";

                return NotFound();
            }
            _catRepo.Remover(categoría);
            _catRepo.Grabar();
            TempData[WC.Exitosa] = "Categoría eliminada exitosamente";

            return RedirectToAction(nameof(Index));

        }

    }
}
