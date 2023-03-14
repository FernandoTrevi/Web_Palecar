﻿using Microsoft.AspNetCore.Mvc;
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

    }
}
