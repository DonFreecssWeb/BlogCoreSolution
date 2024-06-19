using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics;

namespace BlogCore.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public HomeController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Sliders = _contenedorTrabajo.Slider.GetAll(),
                Articulos = _contenedorTrabajo.Articulo.GetAll()                
            };
            ViewBag.isHome = true;
            return View(homeVM);
        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            var articuloFromDb = _contenedorTrabajo.Articulo.GetById(id);

            if(articuloFromDb != null)
            {
                return View(articuloFromDb);
            }
            return View(null);
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
