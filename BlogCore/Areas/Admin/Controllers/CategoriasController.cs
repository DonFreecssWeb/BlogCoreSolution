using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(CNT.Administrador))]
   
    public class CategoriasController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public CategoriasController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        //[AllowAnonymous]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Categoria.Add(categoria);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));

            }
            return View(); ;
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            Categoria categoria =  _contenedorTrabajo.Categoria.GetById(id.Value);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Categoria.Update(categoria);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));

            }
            return View(); 
        }

        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new {data = _contenedorTrabajo.Categoria.GetAll()});
        }

        //ajax para usar el sweetalert

        [HttpDelete]
        public IActionResult Delete(int id)
        {

            var categoriaFromDb = _contenedorTrabajo.Categoria.GetById(id);
            if(categoriaFromDb == null)
            {
                return Json(new {success = false, message = "Error borrando categoria"});
            }

            _contenedorTrabajo.Categoria.Remove(categoriaFromDb);
            _contenedorTrabajo.Save();

            return Json(new {success = true, message = "Categoría borrada correctamente"});
        }

        #endregion
    }
}
