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

        [HttpGet]
        public IActionResult GetNotifications() {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Name));
            var notifications = _context.Notificacao
                .Where(n => n.UsuarioId == userId && n.Aberto)
                .OrderBy(n => n.Id)
                .Select(n => new { n.Id, n.Descricao })
                .ToList();

            return Json(notifications);
        }

        // Ação para marcar uma notificação como lida
        [HttpPost]
        public IActionResult MarkAsRead(int id) {
            var notification = _context.Notificacao.Find(id);
            if (notification != null)
            {
                notification.Aberto = false;
                _context.SaveChanges();
            }

            return Ok();
        }
    }
}
