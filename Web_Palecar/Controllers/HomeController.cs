using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Palecar_AccesoADatos.Datos;
using Palecar_Modelos;
using Palecar_Modelos.ViewModels;
using Palecar_Utilidades;
using Palecar_AccesoADatos.Datos.Repositorio.IRepositorio;

namespace Web_Palecar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductoRepositorio _productoRepo;
        private readonly ICategoriaRepositorio _categoriaRepo;

        public HomeController(ILogger<HomeController> logger,
                            IProductoRepositorio productoRepo,
                            ICategoriaRepositorio categoriaRepo)
        {
            _logger = logger;
            _categoriaRepo = categoriaRepo;
            _productoRepo = productoRepo;
        }

        public IActionResult Index()
        {
            HomeVM homevm = new HomeVM()
            {
                //productos = _db.Productos.Include(c => c.Categoría).Include(t => t.TipoAplicacion),
                //categorías = _db.Categoria
                productos = _productoRepo.ObtenerTodos(incluirPropiedades: "Categoría,TipoAplicacion"),
                categorías = _categoriaRepo.ObtenerTodos()
            };
            return View(homevm);
        }
        public IActionResult Detalle(int Id)
        {
            List<CarroCompra> carroComprasLista = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            };
            DetalleVM detallevm = new DetalleVM()
            {

                //producto = _db.Productos.Include(c => c.Categoría).Include(t => t.TipoAplicacion)
                //                       .Where(p => p.Id == Id).FirstOrDefault(),
                producto = _productoRepo.ObtenerPrimero(p => p.Id == Id),
                ExisteEnCarro = false,
                productosLista = _productoRepo.ObtenerTodos(incluirPropiedades: "Categoría,TipoAplicacion"),



            };
            foreach (var item in carroComprasLista)
            {
                if (item.ProductoId == Id)
                {
                    detallevm.ExisteEnCarro = true;
                    
                }
            }
            return View(detallevm);
        }

        [HttpPost, ActionName("Detalle")]
        public IActionResult DetallePost(int Id, DetalleVM detalleVM)
        {
            List<CarroCompra> carroComprasLista = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            };
            carroComprasLista.Add(new CarroCompra { ProductoId = Id, Cantidad=detalleVM.producto.TempCantidad });
            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasLista);
            TempData[WC.Exitosa] = "Producto agregado al carro exitosamente!";

            return RedirectToAction("Index");
        }

        public IActionResult RemoverDeCarro(int Id)
        {
            List<CarroCompra> carroComprasLista = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            };
            var productoARemover = carroComprasLista.SingleOrDefault(x => x.ProductoId == Id);
            if (productoARemover != null)
            {
                carroComprasLista.Remove(productoARemover);
            }
            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasLista);
            return RedirectToAction("Index");
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