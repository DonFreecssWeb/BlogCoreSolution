using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    
    internal class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoriaRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
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
