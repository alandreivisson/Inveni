using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inveni.Models;
using Inveni.Persistence;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Inveni.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace Inveni.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Contexto _context;

        public UsuariosController(Contexto context)
        {
            _context = context;
        }

        // GET: Usuarios
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Index()
        {
            return _context.Usuario != null ?
                        View(await _context.Usuario.ToListAsync()) :
                        Problem("Entity set 'Contexto.Usuario'  is null.");
        }

        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Senha")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Senha")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuario == null)
            {
                return Problem("Entity set 'Contexto.Usuario'  is null.");
            }
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return (_context.Usuario?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
        [Authorize(Policy = "Anonimo")]
        public IActionResult Acesso()
        {
            return View();
        }

        [HttpPost]
        [Authorize (Policy = "Anonimo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Acesso(UsuarioVM login)
        {
            //Metodo de criptografia
            //var senhaCriptografada = Funcoes.Criptografar(login.Senha);
            Usuario usu = _context.Usuario.Where( t => t.Email == login.Email).FirstOrDefault();

            
            // Verificar credenciais e autenticar o usuário
            if (usu != null)
            {
                if (login.Opcoes.Equals("2"))
                {
                    if (_context.UsuarioPerfil.Any(up => up.UsuarioId == usu.Id && (up.PerfilId == 2 || up.PerfilId == 1)))
                    {
                        usu = null;
                        usu = _context.Usuario
                        .Where(t => t.Email == login.Email && t.Senha == login.Senha)
                        .Join(_context.UsuarioPerfil
                            .Where(usuarioPerfil => usuarioPerfil.PerfilId == 1 || usuarioPerfil.PerfilId == 2),
                            usuario => usuario.Id,
                            usuarioPerfil => usuarioPerfil.UsuarioId,
                            (usuario, usuarioPerfil) => usuario)
                        .FirstOrDefault();

                        if (usu != null && !usu.Ativo) 
                        { 
                            ModelState.AddModelError("", "Usuário está com o perfil inativado, contatar suporte para mais informações!");
                            return View();
                        }
                    }
                    else 
                    {
                        ModelState.AddModelError("", "Usuário não está cadastrado como Mestre!");
                        return View();
                    }
                }
                else
                {
                    if (_context.UsuarioPerfil.Any(up => up.UsuarioId == usu.Id && (up.PerfilId == 3 || up.PerfilId == 1)))
                    {
                        usu = null;
                        usu = _context.Usuario
                        .Where(t => t.Email == login.Email && t.Senha == login.Senha)
                        .Join(_context.UsuarioPerfil
                            .Where(usuarioPerfil => usuarioPerfil.PerfilId == 1 || usuarioPerfil.PerfilId == 3),
                            usuario => usuario.Id,
                            usuarioPerfil => usuarioPerfil.UsuarioId,
                            (usuario, usuarioPerfil) => usuario)
                        .FirstOrDefault();

                        if (usu != null && !usu.Ativo)
                        {
                            ModelState.AddModelError("", "Usuário está com o perfil inativado, contatar suporte para mais informações!");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Usuário não está cadastrado como Aprendiz!");
                        return View();
                    }
                }
                if (usu != null)
                {
                    var permissoes = _context.UsuarioPerfil
                    .Where(up => up.UsuarioId == usu.Id)
                    .Join(
                        _context.Perfil,
                        up => up.PerfilId,
                        perfil => perfil.Id,
                        (up, perfil) => new
                        {
                            PerfilId = perfil.Id
                        }
                    )
                    .Select(result => result.PerfilId).FirstOrDefault().ToString();

                    var claims = new List<Claim>
                     {
                        new Claim(ClaimTypes.Name, usu.Id.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, usu.Nome),
                        new Claim("Permissoes", permissoes)
                // Adicione outras reivindicações conforme necessário
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = false // Defina como verdadeiro se desejar manter o usuário autenticado
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Usuário ou Senha Inválidos!");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Usuário não encontrado!");
                return View();
            }
        }
        public IActionResult ErrorPermissaoAcesso()
        {
            return View();
        }
        public IActionResult ErrorPaginaNaoEncontrada()
        {
            return View();
        }
    }
}
