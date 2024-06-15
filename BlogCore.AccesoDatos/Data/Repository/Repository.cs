using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        internal DbSet<T> dbSet;
        public Repository(DbContext context) { 
            _context = context;
            dbSet = context.Set<T>();
        }
        public void Add(T entity)
        {
            _context.Add(entity);
        }
        public T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            //tarea filtrada
            if(filter != null)
            {
                query = query.Where(filter);
            }
            //tarea relacionada
            //si quiero la lista de categoria y articulos lo paso como categoria,articulo
            //si quiero tener la lista de 1 tabla, no necesito includeProperties y se retornaría
            //la lista de categorias
            if(includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            //ordenamos los registros si se proporciona
            if(orderBy != null)
            {
                return orderBy(query).ToList();
            }
            //si no se proporciona ordenamiento, devuelve la consulta como lista
            return query.ToList();
        }

     

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach(var property in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }                
            }
            return query.FirstOrDefault();
        }             

        public void Remove(int id)
        {
            T entityToRemove = dbSet.Find(id);
            dbSet.Remove(entityToRemove);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
    }
}
