using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Web_Palecar.Datos;
using Web_Palecar.Models;
using Web_Palecar.Models.ViewModels;

namespace Web_Palecar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AplicationDBContext _db;

        public HomeController(ILogger<HomeController> logger, AplicationDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homevm = new HomeVM()
            {
                productos = _db.Productos.Include(c => c.Categoría).Include(t => t.TipoAplicacion),
                categorías = _db.Categoria
            };
            return View(homevm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}