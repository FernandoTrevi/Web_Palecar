﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Web_Palecar.Datos;
using Web_Palecar.Models;
using Web_Palecar.Models.ViewModels;
using Web_Palecar.Utilidades;

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

                producto = _db.Productos.Include(c => c.Categoría).Include(t => t.TipoAplicacion)
                                       .Where(p => p.Id == Id).FirstOrDefault(),
                ExisteEnCarro = false,
                productosLista = _db.Productos.Include(c => c.Categoría).Include(t => t.TipoAplicacion),


            };
            foreach (var item in carroComprasLista)
            {
                if (item.ProductoId== Id)
                {
                    detallevm.ExisteEnCarro= true;
                }
            }
            return View(detallevm);
        }

        [HttpPost, ActionName("Detalle")]
        public IActionResult DetallePost(int Id)
        {
            List<CarroCompra> carroComprasLista = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras)!= null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count()>0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            };
            carroComprasLista.Add(new CarroCompra { ProductoId = Id });
            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasLista);
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