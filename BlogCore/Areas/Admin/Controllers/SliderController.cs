using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;

using Microsoft.AspNetCore.Mvc;

using System.Net;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SliderController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {            

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create( SliderVM sliderVM)
        {
            if(ModelState.IsValid)
            {
                //D:\udemy\Master en ASP.NET MVC\BlogCoreSolution\BlogCore\wwwroot
                string rutaPrincipal = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                  
                //si se envió un archivo
                if(files.Count > 0)
                {
                    string nameGuid = Guid.NewGuid().ToString();
                    var ruta = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                    var extension = Path.GetExtension(files[0].FileName);
                    
                    //creacion física del archivo en wwwroot
                    using(var fileStreams = new FileStream(Path.Combine(ruta, nameGuid + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    Slider slider = new Slider()
                    {
                        Nombre = sliderVM.Nombre,
                        UrlImagen = @"\imagenes\sliders\"+ nameGuid + extension,
                        Estado = sliderVM.Estado
                    };
                    

                    _contenedorTrabajo.Slider.Add(slider);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("UrlImagen", "La imagen es obligatoria");                        
                }             

            }
            //Si todo falla volvemos al la vista Create
          
            return View(sliderVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id != null)
            {
                var slider = _contenedorTrabajo.Slider.GetById(id.GetValueOrDefault());
                SliderVM sliderVM = new SliderVM()
                {
                    Id = slider.Id,
                    Estado = slider.Estado,
                    Nombre = slider.Nombre,
                    UrlImagen = slider.UrlImagen,
                };
                return View(sliderVM);
            }            
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SliderVM sliderVM)
        {
            if (ModelState.IsValid)
            {
                //D:\udemy\Master en ASP.NET MVC\BlogCoreSolution\BlogCore\wwwroot
                string rutaPrincipal = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;               

                var sliderFromDb = _contenedorTrabajo.Slider.GetById(sliderVM.Id);
                 

                //si se subió una nueva imagen
                if (files.Count > 0)
                {
                    string nameGuid = Guid.NewGuid().ToString();
                    var ruta = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                    var extension = Path.GetExtension(files[0].FileName);
                    
                    var rutaImagen = Path.Combine(rutaPrincipal,sliderFromDb.UrlImagen.TrimStart('\\'));

                    //Si existe el archivo en el almacer lo borramos
                    if(System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    //cargamos el nuevo archivo en el almacen
                    using (var fileStreams = new FileStream(Path.Combine(ruta, nameGuid + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    sliderFromDb.Nombre = sliderVM.Nombre;
                    sliderFromDb.UrlImagen = @"imagenes\sliders\" + nameGuid + extension;
                    sliderFromDb.Estado = sliderVM.Estado;


                    _contenedorTrabajo.Slider.Update(sliderFromDb);
                    _contenedorTrabajo.Save();

                    //borramos el anterior del wwwroot


                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Si no queremos subir una imagen nueva
                    sliderFromDb.Nombre = sliderVM.Nombre;
                    sliderFromDb.Estado = sliderVM.Estado;

                    _contenedorTrabajo.Slider.Update(sliderFromDb);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }                

            }
            //Si no se valida las propiedades volvemos a la vista

            return View(sliderVM);
        }

        #region API - Slider
        [HttpGet]
        public IActionResult GetAll()
        {
            var sliders = _contenedorTrabajo.Slider.GetAll();
            return Json(new {data = sliders});
            
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return Json(new { success = false, message = "Falta el identificador" });
            }

            Slider sliderFromDB = _contenedorTrabajo.Slider.GetById(id.Value);
            if(sliderFromDB == null)
            {
                return Json(new { success = false, message = "No hubo coincidencias" });
            }
            else
            {
                _contenedorTrabajo.Slider.Remove(sliderFromDB);
                _contenedorTrabajo.Save();

                //obtenemos la ruta fisica para borrarlo del almacen
                string rutaAlmacenRoot = _webHostEnvironment.WebRootPath;
                
                string rutaFinal = Path.Combine(rutaAlmacenRoot, sliderFromDB.UrlImagen.TrimStart('\\'));

                if (System.IO.File.Exists(rutaFinal))
                {
                    System.IO.File.Delete(rutaFinal);
                }
                return Json(new { success = true, message = "Slider borrado correctamente" });
            }



        }
        
        #endregion
    }
}
