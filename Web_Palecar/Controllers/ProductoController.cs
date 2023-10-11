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
using Palecar_AccesoADatos.Datos.Repositorio.IRepositorio;

namespace Web_Palecar.Controllers
{
    [Authorize(Roles = WC.AdminRole)]

    public class ProductoController : Controller
    {
        private readonly IProductoRepositorio _productoRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductoController(IProductoRepositorio productoRepo, IWebHostEnvironment webHostEnvironment)
        {
            _productoRepo = productoRepo;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            //IEnumerable<Producto> lista = _db.Productos.Include(p => p.Categoría)
            //                                           .Include(t => t.TipoAplicacion);

            IEnumerable<Producto> lista = _productoRepo.ObtenerTodos(incluirPropiedades: "Categoría,TipoAplicacion");
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
                //CategoriaLista = _db.Categoria.Select(c => new SelectListItem
                //{
                //    Text = c.NombreCategoria,
                //    Value = c.Id.ToString()
                //}),
                // TipoAplicacionLista = _db.TipoAplicacions.Select(t => new SelectListItem
                // {
                //     Text = t.Nombre,
                //     Value = t.Id.ToString()
                // })
                CategoriaLista = _productoRepo.ObtenerTodosDropDownList(WC.CategoriaNombre),
                TipoAplicacionLista = _productoRepo.ObtenerTodosDropDownList(WC.TipoAplcacionNombre)

        };

            
            if (id == null)
            {
                return View(productoVM);  
            }
            else
            {
                productoVM.producto = _productoRepo.Obtener(id.GetValueOrDefault());
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
                    _productoRepo.Agregar(productovm.producto);
                    TempData[WC.Exitosa] = "Nuevo Producto creada exitosamente";

                }
                else
                {
                    var objProducto = _productoRepo.ObtenerPrimero(p => p.Id == productovm.producto.Id, isTracking: false);
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
                    _productoRepo.Actualizar(productovm.producto);
                    TempData[WC.Exitosa] = "El producto se editó exitosamente";

                }
                _productoRepo.Grabar();
                return RedirectToAction("Index");
            }// if ModelIsValid
            productovm.CategoriaLista = _productoRepo.ObtenerTodosDropDownList(WC.CategoriaNombre);
            productovm.TipoAplicacionLista = _productoRepo.ObtenerTodosDropDownList(WC.TipoAplcacionNombre);

            return View(productovm);

        }
        public IActionResult Eliminar (int? id)
            //GET
        {
            if (id == null || id == 0) 
            {
                return NotFound();
            }
            Producto producto = _productoRepo.ObtenerPrimero(f=> f.Id == id, incluirPropiedades: "Categoría,TipoAplicacion");
            if (producto == null)
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
                TempData[WC.Error] = "Error al eliminar el producto";

                return NotFound();
            }
            //eliminamos la imagen
            string upload = _webHostEnvironment.WebRootPath + WC.ImagenRuta;
  
            var anteriorFile = Path.Combine(upload, producto.UrlImagen);
            if (System.IO.File.Exists(anteriorFile))
            {
                System.IO.File.Delete(anteriorFile);
            }

            _productoRepo.Remover(producto);           //eliminamos el producto
            _productoRepo.Grabar();
            TempData[WC.Exitosa] = "Producto eliminado exitosamente";

            //guardamos los cambios en el dbContext
            return RedirectToAction(nameof(Index));   //redireccionamos al index
        }

    }
}
