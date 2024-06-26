﻿using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(CNT.Administrador))]
    public class UsuariosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public UsuariosController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //Opción 1: Obtener todos los usuarios
            //return View(_contenedorTrabajo.Usuario.GetAll());
            //Opción 2: Obtener todos los usuarios menos el que esté logueado para no bloquearse a si mismo
            var claimsIdentity = (ClaimsIdentity) this.User.Identity;
            var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(_contenedorTrabajo.Usuario.GetAll(u => u.Id != usuarioActual.Value));
        }
        [HttpGet]
        public IActionResult Bloquear(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.BloquearUsuario(id);
            _contenedorTrabajo.Save();

            return RedirectToAction(nameof(Index)); 
        }

        [HttpGet]
        public IActionResult Desbloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.DesbloquearUsuario(id);
            _contenedorTrabajo.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
