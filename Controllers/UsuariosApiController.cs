using Inveni.Models;
using Inveni.Persistence;
using Inveni.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Inveni.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosApiController : Controller {
        private readonly ILogger<UsuariosApiController> _logger;
        private readonly Contexto _context;

        public UsuariosApiController(ILogger<UsuariosApiController> logger, Contexto context) {
            _logger = logger;
            _context = context;
        }

        [HttpPost("NotificacoesJson")]
        public async Task<IActionResult> GetNotificationsJson([FromBody] NotificacaoRequest request) {
            var notifications = _context.Notificacao
                .Where(n => n.UsuarioId == request.Id && n.Aberto);

            var result = await notifications
               .Select(t => new
               {
                  t.Id,
                  t.Descricao
               })
               .OrderBy(t => t.Id)
               .ToListAsync();


            return Ok(result);
        }

        [HttpPost("FecharNotificacao")]
        public async Task<IActionResult> MarkAsReadJson([FromBody]NotificacaoRequest request) {
            var notification = _context.Notificacao.Find(request.Id);
            if (notification != null)
            {
                notification.Aberto = false;
                _context.SaveChanges();
                return Json(new { status = true });
            }
            else {
                return Json(new { status = false });
            }
        }



        [HttpPost("LoginApiJson")]
        public async Task<IActionResult> Acesso([FromBody] UsuarioVM login) {
            Usuario usu = _context.Usuario.FirstOrDefault(t => t.Email == login.Email);

            // Variáveis de controle para o resultado final
            bool success = false;
            string message = "";
            string permissoes = null;
            object usuario = null;

            // Verificar se o usuário existe
            if (usu != null)
            {
                // Verificar se as credenciais correspondem aos perfis permitidos
                if (login.Opcoes == "3" && _context.UsuarioPerfil.Any(up => up.UsuarioId == usu.Id && (up.PerfilId == 3)))
                {
                    // Verificar se a senha fornecida corresponde ao hash armazenado no banco de dados
                    if (BCrypt.Net.BCrypt.Verify(login.Senha, usu.Senha))
                    {
                        // Verificar se o usuário está ativo
                        if (!usu.Ativo)
                        {
                            message = "Usuário está com o perfil inativado, contatar suporte para mais informações!";
                        }
                        else
                        {
                            // Obter as permissões do usuário
                            permissoes = _context.UsuarioPerfil
                                .Where(up => up.UsuarioId == usu.Id)
                                .Join(
                                    _context.Perfil,
                                    up => up.PerfilId,
                                    perfil => perfil.Id,
                                    (up, perfil) => perfil.Id.ToString()
                                )
                                .FirstOrDefault();

                            // Criar as reivindicações para o usuário autenticado
                            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usu.Id.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, usu.Nome),
                        new Claim("Permissoes", permissoes),
                        new Claim("CaminhoFoto", usu.CaminhoFoto)
                    };

                            // Criar a identidade e propriedades de autenticação
                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var authProperties = new AuthenticationProperties
                            {
                                IsPersistent = false // Defina como verdadeiro se desejar manter o usuário autenticado
                            };

                            // Autenticar o usuário
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                            // Configurar o resultado de autenticação para sucesso e incluir as reivindicações no retorno JSON
                            success = true;
                            message = "Autenticação bem-sucedida!";
                            usuario = new
                            {
                                Id = usu.Id,
                                CaminhoFoto = usu.CaminhoFoto,
                                Nome = usu.Nome
                            };
                        }
                    }
                    else
                    {
                        message = "Usuário ou Senha Inválidos!";
                    }
                }
                else
                {
                    message = login.Opcoes == "2" ? "Usuário não está cadastrado como Mestre!" : "Usuário não está cadastrado como Aprendiz!";
                }
            }
            else
            {
                message = "O E-mail não está cadastrado!";
            }

            // Criar o objeto de resultado final
            var result = new
            {
                Success = success,
                Message = message,
                Permissoes = permissoes,
                Usuario = usuario
            };

            // Retornar o resultado da autenticação como JSON
            return Json(result);
        }

    }
}
