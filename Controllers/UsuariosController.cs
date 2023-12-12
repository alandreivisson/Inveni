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
using Microsoft.CodeAnalysis.Scripting;

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
                        View(await _context.Usuario.OrderBy(u => u.Nome).ToListAsync()) :
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
        public async Task<IActionResult> Edit(string id) {
            int encodeId = Funcoes.DecodeId(id);
            if (encodeId == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(encodeId);
            if (usuario == null)
            {
                return NotFound();
            }

            ViewData["UsuarioNome"] = _context.Usuario.FirstOrDefault(t => t.Id == encodeId)?.Nome;
            ViewData["UsuarioEmail"] = _context.Usuario.FirstOrDefault(t => t.Id == encodeId)?.Email;
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(string? id, [Bind("Id, Email, Nome, Ativo, Senha")] Usuario usuario)
        {
            int decodeId = Funcoes.DecodeId(id);
            if (decodeId != usuario.Id)
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
            ViewData["UsuarioNome"] = _context.Usuario.FirstOrDefault(t => t.Id == decodeId)?.Nome;
            ViewData["UsuarioEmail"] = _context.Usuario.FirstOrDefault(t => t.Id == decodeId)?.Email;
            return View(usuario);
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
        [Authorize(Policy = "Anonimo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Acesso(UsuarioVM login)
        {
            // Buscar o usuário pelo email
            Usuario usu = _context.Usuario.FirstOrDefault(t => t.Email == login.Email);

            // Verificar se o usuário existe
            if (usu != null)
            {
                // Verificar se as credenciais correspondem aos perfis permitidos
                if ((login.Opcoes == "2" && _context.UsuarioPerfil.Any(up => up.UsuarioId == usu.Id && (up.PerfilId == 2 || up.PerfilId == 1))) ||
                    (login.Opcoes != "2" && _context.UsuarioPerfil.Any(up => up.UsuarioId == usu.Id && (up.PerfilId == 3 || up.PerfilId == 1))))
                {
                    // Verificar se a senha fornecida corresponde ao hash armazenado no banco de dados
                    if (BCrypt.Net.BCrypt.Verify(login.Senha, usu.Senha))
                    {
                        // Verificar se o usuário está ativo
                        if (!usu.Ativo)
                        {
                            ModelState.AddModelError("", "Usuário está com o perfil inativado, contatar suporte para mais informações!");
                            return View();
                        }

                        // Obter as permissões do usuário
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
                            .Select(result => result.PerfilId.ToString())
                            .FirstOrDefault();

                        // Criar as reivindicações para o usuário autenticado
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usu.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, usu.Nome),
                    new Claim("Permissoes", permissoes),
                    new Claim("CaminhoFoto", usu.CaminhoFoto)
                    // Adicione outras reivindicações conforme necessário
                };

                        // Criar a identidade e propriedades de autenticação
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {

                            IsPersistent = false // Defina como verdadeiro se desejar manter o usuário autenticado
                        };

                        // Autenticar o usuário
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        // Redirecionar para a página inicial
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", login.Opcoes == "2" ? "Usuário não está cadastrado como Mestre!" : "Usuário não está cadastrado como Aprendiz!");
                    return View();
                }
            }

            ModelState.AddModelError("", "Usuário ou Senha Inválidos!");
            return View();
        }
        public IActionResult ErrorPermissaoAcesso()
        {
            return View();
        }
        public IActionResult ErrorPaginaNaoEncontrada()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Alterar()
        {
            var usuarioLogado = await _context.Usuario.FindAsync(int.Parse(User.Identity.Name));

            if (usuarioLogado == null)
            {
                return NotFound();
            }

            var usuario = new AlterarPerfilVM()
            {
                Nome = usuarioLogado.Nome,
                Email = usuarioLogado.Email,
                Telefone = usuarioLogado.Telefone,
                Biografia = usuarioLogado.Biografia,
                CaminhoFoto = usuarioLogado.CaminhoFoto
            };

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Alterar([FromForm] AlterarPerfilVM usuario) {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            var usuarioBD = await _context.Usuario.FindAsync(int.Parse(User.Identity.Name));

            if (usuarioBD == null)
            {
                return NotFound();
            }

            // Verifique se a pasta do usuário existe, se não, crie
            var userIdFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens", usuarioBD.Id.ToString());
            if (!Directory.Exists(userIdFolder))
            {
                Directory.CreateDirectory(userIdFolder);
            }

            // Salve o arquivo da foto no diretório do usuário
            if (usuario.Foto != null)
            {
                var fileName = Path.GetFileName(usuario.Foto.FileName);
                var filePath = Path.Combine(userIdFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await usuario.Foto.CopyToAsync(fileStream);
                }
                // Atualize o caminho da foto no usuário
                usuarioBD.CaminhoFoto = $"~/imagens/{usuarioBD.Id}/{fileName}";

            }

            var oldClaim = User.Claims.FirstOrDefault(c => c.Type == "CaminhoFoto");
            if (oldClaim != null)
            {
                var identity = (ClaimsIdentity)User.Identity;
                identity.RemoveClaim(oldClaim);
            }

            // Adicionar a nova claim com o caminho da foto atualizado
            var newClaim = new Claim("CaminhoFoto", usuarioBD.CaminhoFoto);
            ((ClaimsIdentity)User.Identity).AddClaim(newClaim);

            // Atualizar a identidade do usuário
            await HttpContext.SignInAsync(User);

            usuarioBD.Nome = usuario.Nome;
            usuarioBD.Telefone = usuario.Telefone;
            usuarioBD.Biografia = usuario.Biografia;

            _context.Update(usuarioBD);
            await _context.SaveChangesAsync();

            // Redireciona para a página inicial
            return RedirectToAction("Index", "Home");
        }

         [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        // POST: /Usuarios/Cadastrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar([Bind("Nome,Localizacao,Email,Senha")] Usuario usuario)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!usuario.ValidarPasswordComplexity())
            {
                ModelState.AddModelError("Senha", "A senha deve ter pelo menos 8 caracteres, uma letra minúscula, uma letra maiúscula, um dígito e um caractere especial.");
                return View();
            }

            if (await _context.Usuario.AnyAsync(u => u.Email == usuario.Email))
            {
                ModelState.AddModelError("Email", "Este endereço de e-mail já está registrado.");
                return View();
            }

            if (usuario is null)
            {
                return BadRequest("O formulário não foi preenchido corretamente.");
            }

            usuario.Ativo = true;

            if (!string.IsNullOrEmpty(usuario.Senha))
            {
                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
            }

            await _context.Usuario.AddAsync(usuario).ConfigureAwait(false);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            int perfilId;
            if (int.TryParse(Request.Form["perfilId"], out perfilId))
            {

                int usuarioId = usuario.Id;

                UsuarioPerfil usuarioPerfil = new UsuarioPerfil
                {
                    UsuarioId = usuarioId,
                    PerfilId = perfilId
                };

                await _context.UsuarioPerfil.AddAsync(usuarioPerfil);

                await _context.SaveChangesAsync();
            }


            return RedirectToAction("Index");
        }
    }
}
