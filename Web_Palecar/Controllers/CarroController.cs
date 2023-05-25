using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web_Palecar.Datos;
using Web_Palecar.Models;
using Web_Palecar.Models.ViewModels;
using Web_Palecar.Utilidades;

namespace Web_Palecar.Controllers
{
    [Authorize]
    public class CarroController : Controller
    {
        private readonly AplicationDBContext _db;

        [BindProperty]//propiedad para que se pueda usar el VM en todo el controlador
        public ProductoUsuarioVM productoUsuarioVM { get; set; }//Variable para poder usar el VM creado

        public CarroController(AplicationDBContext db)
        {
           _db=db; 
        }
        public IActionResult Index()
        {
            List<CarroCompra> carroComprasList = new List<CarroCompra>();
            if(HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras)!=null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count()> 0)
            {
                carroComprasList = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            List<int> ProdInCarro = carroComprasList.Select(i=> i.ProductoId).ToList();
            IEnumerable<Producto> prodList = _db.Productos.Where(p => ProdInCarro.Contains(p.Id));
               
            return View(prodList);
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Resumen));
        }
        
        public IActionResult Resumen()
        {
            //capturamos el usuario conectado
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //Obtenemos la lista de productos slecionada por el usuario en la session
            List<CarroCompra> carroComprasList = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasList = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            List<int> ProdInCarro = carroComprasList.Select(i => i.ProductoId).ToList();
            IEnumerable<Producto> prodList = _db.Productos.Where(p => ProdInCarro.Contains(p.Id));

            //llenamos el VM con los datos de prodList y del usuario capturado por claim
            productoUsuarioVM = new ProductoUsuarioVM()
            {
                usuariosAplicacion = _db.UsuariosAplicacions.FirstOrDefault(u => u.Id == claim.Value),
                productoLista = prodList
            };
            return View(productoUsuarioVM);
        }

        public IActionResult Remover(int Id)
        {
            List<CarroCompra> carroComprasList = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasList = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            carroComprasList.Remove(carroComprasList.FirstOrDefault(p=> p.ProductoId == Id));
            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasList);
            return RedirectToAction(nameof(Index));
        }
    }
}
