using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using Web_Palecar.Datos;
using Web_Palecar.Models;
using Web_Palecar.Models.ViewModels;
using Web_Palecar.Utilidades;

namespace Web_Palecar.Controllers
{
    [Authorize]
    public class CarroController : Controller
    {
        [BindProperty]
        public ProductoUsuarioVM productoUsuarioVM { get; set; }
        private readonly AplicationDBContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        public CarroController(AplicationDBContext db, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
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
                productoLista = prodList.ToList(),
            };
            return View(productoUsuarioVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Resumen")]
        public async Task<IActionResult> ResumenPost(ProductoUsuarioVM productoUsuarioVM)
        {
            var templatePath = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                                                               + "templates"
                                                               + Path.DirectorySeparatorChar.ToString()
                                                               + "PlantillaOrden.html";
            var subjet = "Nueva orden";
            var htmlBody = "";

            using (StreamReader sr = System.IO.File.OpenText(templatePath))
            {
                htmlBody = sr.ReadToEnd();
            }

            StringBuilder productoListaSb = new StringBuilder();

            foreach (var prod in productoUsuarioVM.productoLista)
            {
                productoListaSb.Append($" - Nombre: {prod.NombreProducto} <span style=´font-size:14px;´>(Id: {prod.Id})<span/><br/>");
            }

            string msgBody = string.Format(htmlBody,
                                           productoUsuarioVM.usuariosAplicacion.NombreCompleto,
                                           productoUsuarioVM.usuariosAplicacion.Email,
                                           productoUsuarioVM.usuariosAplicacion.PhoneNumber,
                                           productoListaSb.ToString());

            await _emailSender.SendEmailAsync(WC.EmailAdmin, subjet, msgBody);



            return RedirectToAction(nameof(Confirmacion));
        }

        public IActionResult Confirmacion()
        {
            HttpContext.Session.Clear();
            return View();
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
