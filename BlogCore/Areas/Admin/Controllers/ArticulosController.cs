using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
namespace BlogCore.Areas.Admin.Controllers
{
     [Area("Admin")]
    [Authorize(Roles = nameof(CNT.Administrador))]
    public class ArticulosController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IContenedorTrabajo _contenedorTrabajo;
        public ArticulosController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: ArticulosController
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // GET: ArticulosController/Create
        [HttpGet]
        public ActionResult Create()
        {
            ArticuloVM artiVM = new ArticuloVM()
            {
                Articulo = new Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };
            return View(artiVM);
        }

        // POST: ArticulosController/Create
        //recibe articulo + categoria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( ArticuloVM articuloVM)
        {
           
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _webHostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                //validamos si se subió un archivo y si se está creando un artículo nuevo
                if(articuloVM.Articulo.Id == 0 )
                {
                    if(archivos.Count() > 0) { 
                    //nuevo artículo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }
                    articuloVM.Articulo.UrlImagen = @"imagenes\articulos\"+nombreArchivo + extension;
                    articuloVM.Articulo.FechaCreacion = DateTime.Now;


                    _contenedorTrabajo.Articulo.Add(articuloVM.Articulo);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("imagen","Debes seleccionar una imagen");
                    }
                }
            }   
            //si todo falla se regresa al Create GET y pasamos el artiVM
            //para no perder la lista de categorias para el dropdown
            articuloVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
                return View(articuloVM);            
        }

        // GET: ArticulosController/Edit/5
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticuloVM artiVM = new ArticuloVM()
            {
                Articulo = new Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };
            if(id != null)
            {
                artiVM.Articulo = _contenedorTrabajo.Articulo.GetById(id.GetValueOrDefault());
            }
            return View(artiVM);
        }

        // POST: ArticulosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit( ArticuloVM articuloVM)
        {
            ModelState.Remove("Articulo.Categoria");
            ModelState.Remove("Articulo.UrlImagen");
            ModelState.Remove(nameof(ArticuloVM.ListaCategorias));
            string decodificado = WebUtility.HtmlDecode(articuloVM.Articulo.Descripcion);
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _webHostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                var articuloToUpdate = _contenedorTrabajo.Articulo.GetById(articuloVM.Articulo.Id);

                
                //validamos si se subió un archivo, porque se quiere reemplazar la imagen
                //cuando se sube un archivo nuevo
                if (archivos.Count() > 0)
                {
                    //nueva imagen para el artículo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                    var rutaImagen = Path.Combine(rutaPrincipal, articuloToUpdate.UrlImagen.TrimStart('\\'));
                    //borrar la imagen anterior
                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }
                    //nuevamente subimos el archivo
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }
                    articuloVM.Articulo.UrlImagen = @"imagenes\articulos\" + nombreArchivo + extension;
                    articuloVM.Articulo.FechaCreacion = DateTime.Now;
                    articuloVM.Articulo.Descripcion = decodificado;

                    _contenedorTrabajo.Articulo.Update(articuloVM.Articulo);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {//cuando no queremos subir un archivo nuevo
                    articuloVM.Articulo.UrlImagen =articuloToUpdate.UrlImagen;
                    articuloVM.Articulo.FechaCreacion = DateTime.Now;
                    articuloVM.Articulo.Descripcion = decodificado;

                }
                _contenedorTrabajo.Articulo.Update(articuloVM.Articulo);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));

            }
            //si todo falla se regresa al Create GET y pasamos el artiVM
            //para no perder la lista de categorias para el dropdown
            articuloVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(articuloVM);
        }

        // GET: ArticulosController/Delete/5
        public ActionResult Delete()
        {
            return View();
        }

        // POST: ArticulosController/Delete/5
     
        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Articulo.GetAll(includeProperties:"Categoria")});
        }

        //ajax para usar el sweetalert

        [HttpDelete]
        public IActionResult Delete(int id)
        {

            var articuloFromDb = _contenedorTrabajo.Articulo.GetById(id);
            //borrarmos la imagen de la carpeta
            string rutaDirectorioPrincipal = _webHostEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, articuloFromDb.UrlImagen.TrimStart('\\'));

            //si existe el archivo lo borramos
            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }

            if (articuloFromDb == null)
            {
                return Json(new { success = false, message = "Error borrando articulo" });
            }

            _contenedorTrabajo.Articulo.Remove(articuloFromDb);
            _contenedorTrabajo.Save();

            return Json(new { success = true, message = "Artículo borrado correctamente" });
        }

        #endregion
    }
}
