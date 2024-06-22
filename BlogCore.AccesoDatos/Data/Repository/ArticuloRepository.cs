using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    
    public class ArticuloRepository : Repository<Articulo>, IArticuloRepository
    {
        private readonly ApplicationDbContext _db;

        public ArticuloRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }

        public IQueryable<Articulo> AsQueryable()
        {
            //Consultable, es parte de LINQ, permite consultar a la base de datos
            //devuelve una instancia para consultar

           //return  _db.Set<Articulo>().AsQueryable();
           return _db.Articulo.AsQueryable();
        }

        public void Update(Articulo articulo)
        {
            var articuloToUpdate = _db.Articulo.FirstOrDefault(s => s.Id == articulo.Id);
            articuloToUpdate.Nombre = articulo.Nombre;
            articuloToUpdate.Descripcion = articulo.Descripcion;
            articuloToUpdate.FechaCreacion = articulo.FechaCreacion;
            articuloToUpdate.UrlImagen = articulo.UrlImagen;
            articuloToUpdate.CategoriaID = articulo.CategoriaID;

            //no se requiere porque la unidad de trabajo hace el save
           // _db.SaveChanges();
        }
    }
}
