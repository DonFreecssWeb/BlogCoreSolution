using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class UsuarioRepository : Repository<ApplicationUser>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public void BloquearUsuario(string UsuarioID)
        {
            var usuarioDb = _context.ApplicationUser.FirstOrDefault(u => u.Id == UsuarioID);
            usuarioDb.LockoutEnd = DateTime.Now.AddYears(1000);
        }

        public void DesbloquearUsuario(string UsuarioID)
        {
            var usuarioDb = _context.ApplicationUser.FirstOrDefault(u => u.Id == UsuarioID);
            usuarioDb.LockoutEnd = DateTime.Now;
        }
    }
}
