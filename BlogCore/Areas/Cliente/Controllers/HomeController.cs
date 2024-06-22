using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics;
using System.Drawing.Printing;

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

        //Pagina de inicio sin paginación
        //public IActionResult Index()
        //{
        //    HomeVM homeVM = new HomeVM()
        //    {
        //        Sliders = _contenedorTrabajo.Slider.GetAll(),
        //        Articulos = _contenedorTrabajo.Articulo.GetAll()                
        //    };
        //    ViewBag.isHome = true;
        //    return View(homeVM);
        //}

        //Pagina de inicio con paginación
        public IActionResult Index(int page = 1, int pageSize = 6)
        {
            var articulos = _contenedorTrabajo.Articulo.AsQueryable();
            //page empieza en 1 por eso lo restamos
            //pageSize es la cantidad de elementos por página
            var paginasEntries = articulos.Skip((page - 1) * pageSize).Take(pageSize);
            HomeVM homeVM = new HomeVM()
            {
                Sliders = _contenedorTrabajo.Slider.GetAll(),
                Articulos = paginasEntries.ToList(),
                PageIndex = page,
                TotalPages = (int)Math.Ceiling(articulos.Count() / (double)pageSize)
             };

            
            ViewBag.isHome = true;
            return View(homeVM);
        }

        //buscador
        [HttpGet]
        public IActionResult ResultadoBusqueda(string searchString, int page = 1, int pageSize = 6)
        {
            var articulos = _contenedorTrabajo.Articulo.AsQueryable();
            if(!string.IsNullOrEmpty(searchString))
            {
                articulos = articulos.Where(e => e.Nombre.Contains(searchString));
            }
            //page empieza en 1 por eso lo restamos
            //pageSize es la cantidad de elementos por página
            var paginasEntries = articulos.Skip((page - 1) * pageSize).Take(pageSize);

            //crear modelo para la vista
            ListaPaginadaVM<Articulo> listaPaginadaVM = new ListaPaginadaVM<Articulo>(paginasEntries.ToList(),articulos.Count(),page,pageSize,searchString);
            return View(listaPaginadaVM);
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
