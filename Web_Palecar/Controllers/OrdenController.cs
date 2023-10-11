using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palecar_AccesoADatos.Datos.Repositorio.IRepositorio;
using Palecar_Modelos;
using Palecar_Modelos.ViewModels;
using Palecar_Utilidades;

namespace Web_Palecar.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class OrdenController : Controller
    {
        private readonly IOrdenRepositorio _ordenRepo;
        private readonly IOrdenDetalleRepositorio _ordenDetalleRepo;

        [BindProperty]
        public OrdenVM OrdenVM { get; set; }

        public OrdenController(IOrdenRepositorio ordenRepo, IOrdenDetalleRepositorio ordenDetalleRepo )
        {
            _ordenRepo = ordenRepo;
            _ordenDetalleRepo = ordenDetalleRepo;
                
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detalle(int Id)
        {
            OrdenVM = new OrdenVM
            {
                Orden = _ordenRepo.ObtenerPrimero(o=>o.Id == Id),
                OrdenDetalle = _ordenDetalleRepo.ObtenerTodos(d=>d.OrdenId == Id, incluirPropiedades:"Producto"),
            };
            return View(OrdenVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Detalle()
        {
            List<CarroCompra>carroComprasLista = new List<CarroCompra>();
            OrdenVM.OrdenDetalle = _ordenDetalleRepo.ObtenerTodos(o => o.OrdenId == OrdenVM.Orden.Id);

            foreach (var detalle in OrdenVM.OrdenDetalle)
            {
                CarroCompra carroCompra = new CarroCompra()
                {
                    ProductoId = detalle.ProductoId,
                };
                carroComprasLista.Add(carroCompra);
            }
            HttpContext.Session.Clear();
            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasLista);
            HttpContext.Session.Set(WC.SessionOrdenId, OrdenVM.Orden.Id);
            return RedirectToAction("Index","Carro");
        }

        [HttpPost]
        public IActionResult Eliminar()
        {
            Orden orden = _ordenRepo.ObtenerPrimero(o=> o.Id == OrdenVM.Orden.Id);
            IEnumerable<OrdenDetalle>ordenDetallesLista = _ordenDetalleRepo.ObtenerTodos(d=> d.OrdenId ==OrdenVM.Orden.Id);

            _ordenDetalleRepo.RemoverRango(ordenDetallesLista);
            _ordenRepo.Remover(orden);
            _ordenRepo.Grabar();
            TempData[WC.Exitosa] = "La Orden fue eliminada exitosamente";

            return RedirectToAction(nameof(Index));
        }

        #region APIs
        [HttpGet]
        public IActionResult ObtenerListaOrdenes()
        {
            return Json(new { data = _ordenRepo.ObtenerTodos() });
        }
        #endregion
    }

}
