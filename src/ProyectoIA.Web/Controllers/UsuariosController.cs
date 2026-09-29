using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoIA.Domain;
using ProyectoIA.Infrastructure;
using ProyectoIA.Web.Models;

namespace ProyectoIA.Web.Controllers;

[Authorize(Roles = nameof(Rol.Administrador))]
public class UsuariosController : Controller
{
    private readonly AppDbContext _db;

    public UsuariosController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var usuarios = await _db.Usuarios
            .OrderBy(u => u.Rol)
            .ThenBy(u => u.Nombre)
            .ToListAsync();
        return View(usuarios);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
            return View(new UsuarioFormViewModel());

        var usuario = await _db.Usuarios.FindAsync(id);
        if (usuario is null)
            return NotFound();

        return View(new UsuarioFormViewModel
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Correo = usuario.Correo,
            Rol = usuario.Rol
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(UsuarioFormViewModel model)
    {
        var correo = model.Correo.Trim().ToLowerInvariant();
        if (await _db.Usuarios.AnyAsync(u => u.Correo == correo && u.Id != model.Id))
            ModelState.AddModelError(nameof(model.Correo), "Ya existe un usuario con ese correo.");
        if (!Enum.IsDefined(model.Rol))
            ModelState.AddModelError(nameof(model.Rol), "Seleccione un rol válido.");
        if (model.Id == 0 && string.IsNullOrWhiteSpace(model.Password))
            ModelState.AddModelError(nameof(model.Password), "La contraseña es obligatoria para un usuario nuevo.");

        if (!ModelState.IsValid)
            return View("Edit", model);

        Usuario usuario;
        if (model.Id == 0)
        {
            usuario = new Usuario();
            _db.Usuarios.Add(usuario);
        }
        else
        {
            usuario = await _db.Usuarios.FindAsync(model.Id) ?? throw new InvalidOperationException("El usuario ya no existe.");
            if (usuario.Id.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier) &&
                model.Rol != Rol.Administrador)
                return BadRequest("No puede quitarse el rol de administrador de su propia cuenta.");
        }

        usuario.Nombre = model.Nombre.Trim();
        usuario.Correo = correo;
        usuario.Rol = model.Rol;
        usuario.Activo = true;
        if (!string.IsNullOrWhiteSpace(model.Password))
            usuario.PasswordHash = PasswordHasher.Hash(model.Password);

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Desactivar(int id)
    {
        var usuario = await _db.Usuarios.FindAsync(id);
        if (usuario is null)
            return NotFound();

        if (usuario.Id.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier))
            return BadRequest("No puede desactivar su propia cuenta.");

        if (usuario.Activo && usuario.Rol == Rol.Administrador &&
            await _db.Usuarios.CountAsync(u => u.Activo && u.Rol == Rol.Administrador) <= 1)
            return BadRequest("Debe quedar al menos un administrador activo.");

        usuario.Activo = false;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
