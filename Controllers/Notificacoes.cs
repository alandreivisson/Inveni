using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Inveni.Models;
using Inveni.Persistence;

namespace Inveni.Controllers {
    [Authorize]
    public class NotificacoesController : Controller {
        private readonly Contexto _context;

        public NotificacoesController(Contexto context) {
            _context = context;
        }

        // Ação para carregar notificações do usuário autenticado
        public async Task<IActionResult> Index() {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Name));
            var notificacoes = await _context.Notificacao
                .Where(n => n.UsuarioId == userId)
                .OrderByDescending(n => n.Id)
                .ToListAsync();

            return View(notificacoes);
        }

        // Ação para marcar uma notificação como lida
        [HttpPost]
        public async Task<IActionResult> MarcarComoLida(int id) {
            var notificacao = await _context.Notificacao.FindAsync(id);
            if (notificacao != null)
            {
                notificacao.Aberto = false;
                await _context.SaveChangesAsync();
                return Ok(); // Retorno OK se sucesso
            }
            return NotFound(); // Retorno NotFound se não encontrou a notificação
        }
    }
}
