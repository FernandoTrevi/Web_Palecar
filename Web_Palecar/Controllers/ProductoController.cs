using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.EntityFrameworkCore;
using Palecar_Utilidades;
using Palecar_Modelos;
using Palecar_Modelos.ViewModels;
using System.IO;
using Palecar_AccesoADatos.Datos;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace Web_Palecar.Controllers
{
    [Authorize(Roles = WC.AdminRole)]

    public class ProductoController : Controller
    {
        private readonly AplicationDBContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductoController(AplicationDBContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Producto> lista = _db.Productos.Include(p => p.Categoría)
                                                       .Include(t => t.TipoAplicacion);
            return View(lista);
        }
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> categoriaDropDawn = _db.Categoria.Select(c => new SelectListItem
            //{
            //    Text = c.NombreCategoria,
            //    Value = c.Id.ToString()
            //});
            //ViewBag.categoriaDropDawn = categoriaDropDawn;
            //Producto producto = new Producto();

            ProductoVM productoVM = new ProductoVM()
            {
                producto = new Producto(),
                CategoriaLista = _db.Categoria.Select(c => new SelectListItem
                {
                    Text = c.NombreCategoria,
                    Value = c.Id.ToString()
                }),
                 TipoAplicacionLista = _db.TipoAplicacions.Select(t => new SelectListItem
                 {
                     Text = t.Nombre,
                     Value = t.Id.ToString()
                 })

        };

            
            if (id == null)
            {
                return View(productoVM);  
            }
            else
            {
                productoVM.producto = _db.Productos.Find(id);
                if (productoVM.producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductoVM productovm)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRoutePath = _webHostEnvironment.WebRootPath;
                if(productovm.producto.Id == 0)
                {
                    string upload = webRoutePath + WC.ImagenRuta;
                    string filename= Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName); 
                    using (var filestream = new FileStream(Path.Combine(upload,filename+extension),FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    productovm.producto.UrlImagen = filename+extension;
                    _db.Productos.Add(productovm.producto);
                }
                else
                {
                    var objProducto = _db.Productos.AsNoTracking().FirstOrDefault(p=> p.Id == productovm.producto.Id);
                    if(files.Count > 0)// si el usuario intenta cambiar la imagen
                    {
                        //creamos la nueva imagen
                        string upload = webRoutePath + WC.ImagenRuta;
                        string filename = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        //borramos la imagen anterior
                        var anteriorFile = Path.Combine(upload,objProducto.UrlImagen);
                        if (System.IO.File.Exists(anteriorFile))
                        {
                            System.IO.File.Delete(anteriorFile);
                        }

                        using (var filestream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                        {
                            files[0].CopyTo(filestream);
                        }
                        //Asignamos la nueva imagen al view model
                        productovm.producto.UrlImagen=filename+extension;
                    }
                    else //en caso de que no haya intentado cambiar la imagen
                    {
                        productovm.producto.UrlImagen = objProducto.UrlImagen;
                    }
                    //actualizamos el producto
                    _db.Productos.Update(productovm.producto);

                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }// if ModelIsValid
            return View(productovm);

        }
        public IActionResult Eliminar (int? id)
            //GET
        {
            if (id == null || id == 0) 
            {
                return NotFound();
            }
            Producto producto = _db.Productos.Include(c => c.Categoría)
                                             .Include(t => t.TipoAplicacion)
                                             .FirstOrDefault(f=> f.Id == id );
            if(producto == null)
            {
                return NotFound();
            }
                                             
            return View(producto);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Producto producto)
        {
            if (producto == null)
            {
                return NotFound();
            }
            //eliminamos la imagen
            string upload = _webHostEnvironment.WebRootPath + WC.ImagenRuta;
  
            var anteriorFile = Path.Combine(upload, producto.UrlImagen);
            if (System.IO.File.Exists(anteriorFile))
            {
                System.IO.File.Delete(anteriorFile);
            }

            _db.Productos.Remove(producto);           //eliminamos el producto
            _db.SaveChanges();                        //guardamos los cambios en el dbContext
            return RedirectToAction(nameof(Index));   //redireccionamos al index
        }

    }
}
