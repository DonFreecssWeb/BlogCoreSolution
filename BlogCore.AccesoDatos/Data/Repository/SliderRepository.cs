using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{

    public class SliderRepository : Repository<Slider>, ISlideRepository
    {
        private readonly ApplicationDbContext _db;

        public SliderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }  
        
        public void Update(Slider slider)
        {
            var sliderFromDB = _db.Slider.FirstOrDefault(s => s.Id == slider.Id);
            sliderFromDB.Nombre = slider.Nombre;
            sliderFromDB.Estado = slider.Estado;
            sliderFromDB.UrlImagen = slider.UrlImagen;            
        }
    }
}
