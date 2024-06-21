using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoriaRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetListaCategorias()
        {
            return _db.Categoria.Select(temp => new SelectListItem()
            {
                Text = temp.Nombre,
                Value = temp.Id.ToString()
            }) ;
        }

        public void Update(Categoria categoria)
        {
            var categoriaToUpdate = _db.Categoria.FirstOrDefault(s => s.Id == categoria.Id);
            categoriaToUpdate.Nombre = categoria.Nombre;
            categoriaToUpdate.Orden = categoria.Orden;

            //no se requiere porque la unidad de trabajo hace el save
            // _db.SaveChanges();
        }
    }
}
