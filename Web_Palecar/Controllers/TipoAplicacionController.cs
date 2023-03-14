﻿using Microsoft.AspNetCore.Mvc;
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
    }
}