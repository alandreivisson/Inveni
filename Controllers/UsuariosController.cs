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
using Inveni.Services;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Inveni.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Contexto _context;
        private readonly IEmailService _emailService;

        public UsuariosController(Contexto context, IEmailService emailService) {
            _context = context;
            _emailService = emailService;
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
            
            ViewData["UsuarioCaminhoFoto"] = _context.Usuario.FirstOrDefault(t => t.Id == encodeId)?.CaminhoFoto;
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
                usuarioBD.CaminhoFoto = $"/imagens/{usuarioBD.Id}/{fileName}";

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
        public async Task<IActionResult> Cadastrar([Bind("Nome,Localizacao,Email,Senha")] Usuario usuario, string termosUso)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (string.IsNullOrEmpty(termosUso))
            {
                ViewData["Mensagem"] = "Você deve aceitar os termos de uso para continuar";
                return View(usuario);
            }

            if (!usuario.ValidarPasswordComplexity())
            {
                ViewData["Mensagem"] = "A senha deve ter pelo menos 8 caracteres, uma letra minúscula, uma letra maiúscula, um dígito e um caractere especial.";
                return View();
            }

            if (await _context.Usuario.AnyAsync(u => u.Email == usuario.Email))
            {
                ViewData["Mensagem"] = "Este endereço de e-mail já está registrado.";
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

            usuario.CaminhoFoto = $"/imagens/user.png";

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


            ViewData["Mensagem"] = "Usuário Cadastrado com sucesso!";
            return View(nameof(Acesso));
        }
        [HttpGet]
        public IActionResult EsqueciSenha() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EsqueciSenha([FromForm] EsqueciSenhaViewModel dados) 
        {
            if (ModelState.IsValid)
            {
                //if (_context.Usuario.AsNoTracking().Any(u => u.Email == dados.Email.ToUpper()))
                //{
                    var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Email == dados.Email);
                    var token = GenerateJwtToken(usuario.Email);
                    var urlConfirmacao = Url.Action(nameof(RedefinirSenha), "Usuarios", new { token }, Request.Scheme);
                    var mensagem = new StringBuilder();
                    mensagem.Append("<p>Olá, {usuario.Nome}. </p>");
                    mensagem.Append("<p>Houve uma solicitacção de redefinição de senha para seu usuário em nosso site. Senão foi você quem fez a solicitação, ignore essa" +
                        " mensagem. Caso tenha sido você, clique no link abaixo para criar sua nova senha:</p>");
                    mensagem.Append($"<p><a href='{urlConfirmacao}'>Redefinir Senha</a></p>");
                    mensagem.Append("<p>Atenciosamente, <br>Equipe EstudeFacil</p>");
                    await _emailService.SendEmailAsync(usuario.Email, "Redefinição de Senha", "", mensagem.ToString());
                    return View(nameof(EmailRedefinicaoEnviado));
                //}
            }
            else {
                return View(dados);
            }
        }
        public IActionResult EmailRedefinicaoEnviado() {
            return View();
        }
        [HttpGet]
        public IActionResult RedefinirSenha(string token) 
        {
            var modelo = new RedefinirSenhaViewModel
            {
                Email = null,
                NovaSenha = null,
                ConfNovaSenha = null,
                Token = token
            };

            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> RedefinirSenha([FromForm] RedefinirSenhaViewModel dados) {
            if (ModelState.IsValid)
            {
                var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Email == dados.Email);
                if (usuario != null)
                {
                    if (ValidarToken(dados.Token, usuario.Email))
                    {
                        try
                        {

                            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(dados.NovaSenha);
                            _context.Usuario.Update(usuario);
                            await _context.SaveChangesAsync();

                            ViewData["Mensagem"] = "Senha redefinida com sucesso!";
                            return View(nameof(Acesso));
                        }
                        catch (Exception ex)
                        {
                            ViewData["Mensagem"] = "Não foi possível redefinir a senha. Verifique se preencheu a senha corretamente. Se o problema persistir, entre em contato com o suporte.";
                            return View(dados);
                        }
                    }
                    else
                    {
                        ViewData["Mensagem"] = "Token inválido ou expirado.";
                        return View(dados);
                    }
                }
                else
                {
                    ViewData["Mensagem"] = "Usuário não encontrado.";
                    return View(dados);
                }
            }
            else
            {
                return View(dados);
            }
        }


        private string GenerateJwtToken(string email) {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Funcoes.Hash("@Estudefacil2024"))); // Usando a função Hash

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512); // Alterando para HmacSha512

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "EstudeFacil",
                audience: "EstudeFacil",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private bool ValidarToken(string token, string email) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Funcoes.Hash("@Estudefacil2024")); // Usar a mesma lógica de chave usada no GenerateJwtToken

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var tokenEmail = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;

                return tokenEmail == email;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        public IActionResult TermosDeUso() {
            return View();
        }

    }
}
