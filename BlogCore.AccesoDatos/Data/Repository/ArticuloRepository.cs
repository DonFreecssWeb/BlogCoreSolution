using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    
    internal class ArticuloRepository : Repository<Articulo>, IArticuloRepository
    {
        private readonly ApplicationDbContext _db;

        public ArticuloRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
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
