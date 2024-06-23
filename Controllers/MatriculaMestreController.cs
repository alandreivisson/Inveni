using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inveni.Models;
using Inveni.Persistence;
using Microsoft.AspNetCore.Authorization;

namespace Inveni.Controllers {
    [Authorize(Policy = "Mestre")]
    public class MatriculaMestreController : Controller {
        private readonly Contexto _context;

        public MatriculaMestreController(Contexto context) {
            _context = context;
        }

        // GET: MatriculaMestre
        public async Task<IActionResult> Index() {
            try
            {
                int mestreId = Convert.ToInt32(User.Identity.Name);

                // Consulta as temáticas do mestre ordenadas alfabeticamente
                var tematicasMestre = await _context.TematicaMestre
                 .Include(tm => tm.Tematica)
                 .Include(tm => tm.MatriculaMestre)
                     .ThenInclude(mm => mm.Aprendiz)
                 .Where(tm => tm.UsuarioId == mestreId)
                 .OrderBy(tm => tm.Tematica.Descricao)
                 .ToListAsync();

                // Segundo bloco: Ordena as matrículas dentro de cada temática
                foreach (var tematica in tematicasMestre)
                {
                    tematica.MatriculaMestre = tematica.MatriculaMestre.Where(mm => mm.Status == MatriculaStatus.Matriculado)
                        .OrderBy(mm => mm.Aprendiz.Nome)
                        .ToList();
                }
                return View(tematicasMestre);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                }

                return View("Error");
            }
        }

        public async Task<IActionResult> VisualizarMatriculas(int tematicaId) {
            var mestreId = Convert.ToInt32(User.Identity.Name);

            // Consulta as matrículas matriculadas para uma temática específica
            var matriculasMatriculadas = await _context.MatriculaMestre
                .Include(m => m.Aprendiz)
                .Include(m => m.TematicaMestre)
                    .ThenInclude(tm => tm.Tematica)
                .Where(m => m.Status == MatriculaStatus.Matriculado && m.TematicaMestre.TematicaId == tematicaId && m.MestreId == mestreId)
                .OrderBy(m => m.Aprendiz.Nome)
                .ToListAsync();

            return PartialView("_MatriculasList", matriculasMatriculadas);
        }



        [HttpPost]
        [Authorize(Policy = "Mestre")]
        public ActionResult Confirmar(int id) {
            try
            {
                var matriculaMestre = _context.MatriculaMestre.
                    Include(mm => mm.Mestre).
                    Include(mm => mm.TematicaMestre).
                        ThenInclude(mm => mm.Tematica).
                    FirstOrDefault(mm => mm.Id == id);
                if (matriculaMestre == null)
                {
                    return Json(new { ok = false, message = "Matrícula não encontrada." });
                }

                matriculaMestre.Status = MatriculaStatus.Matriculado;

                var mensagem = $"Matricula confirmada pelo(a) Mestre {matriculaMestre.Mestre.Nome} - {matriculaMestre.TematicaMestre.Tematica.Descricao}";


                var notificacao = new Notificacao
                {
                    Descricao = mensagem,
                    Aberto = true,
                    UsuarioId = matriculaMestre.AprendizId // Define o aprendiz como destinatário da notificação
                };

                _context.Notificacao.Add(notificacao);
                _context.Update(matriculaMestre);
                _context.SaveChanges();

                return Json(new { ok = true });
            }
            catch (Exception ex)
            {
                // Log detalhado do erro
                Console.WriteLine($"Erro ao confirmar matrícula: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                }
                return Json(new { ok = false, message = "Erro ao confirmar matrícula." });
            }
        }



        [HttpPost]
        [Authorize(Policy = "Mestre")]
        public IActionResult Rejeitar(int id) {
            var matriculaMestre = _context.MatriculaMestre.
                    Include(mm => mm.Mestre).
                    Include(mm => mm.TematicaMestre).
                        ThenInclude(mm => mm.Tematica).
                    FirstOrDefault(mm => mm.Id == id);
            if (matriculaMestre == null)
            {
                return Json(new { ok = false });
            }

            var mensagem = $"Matricula rejeitada pelo(a) Mestre {matriculaMestre.Mestre.Nome} - {matriculaMestre.TematicaMestre.Tematica.Descricao}";

            var notificacao = new Notificacao
            {
                Descricao = mensagem,
                Aberto = true,
                UsuarioId = matriculaMestre.AprendizId // Define o aprendiz como destinatário da notificação
            };

            _context.Notificacao.Add(notificacao);
            _context.MatriculaMestre.Remove(matriculaMestre);
            _context.SaveChanges();

            return Json(new { ok = true });
        }

        private bool MatriculaMestreExists(int id) {
            return (_context.MatriculaMestre?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Create() {
            int mestreId = Convert.ToInt32(User.Identity.Name);
            var matriculasPendentes = await _context.MatriculaMestre
                .Include(m => m.Aprendiz)
                .Include(m => m.TematicaMestre)
                .ThenInclude(tm => tm.Tematica)
                .Where(m => m.MestreId == mestreId && m.Status == MatriculaStatus.Pendente)
                .OrderBy(m => m.TematicaMestre.Tematica.Descricao)
                .ThenBy(m => m.Aprendiz.Nome)
                .ToListAsync();

            return View(matriculasPendentes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinalizarMatricula(int id) {
            var matriculaMestre = await _context.MatriculaMestre
                .Include(mm => mm.MaterialMatriculaMestre).
                 Include(mm => mm.Mestre).
                    Include(mm => mm.TematicaMestre).
                        ThenInclude(mm => mm.Tematica)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (matriculaMestre == null)
            {
                return NotFound();
            }

            try
            {
                // Deleta os registros de MaterialMatriculaMestre associados
                foreach (var materialMatriculaMestre in matriculaMestre.MaterialMatriculaMestre.ToList())
                {
                    _context.MaterialMatriculaMestre.Remove(materialMatriculaMestre);
                }

                var mensagem = $"Matricula finalizada pelo(a) Mestre {matriculaMestre.Mestre.Nome} - {matriculaMestre.TematicaMestre.Tematica.Descricao}, todos os materiais enviados por ele não estão mais disponíveis!";

                var notificacao = new Notificacao
                {
                    Descricao = mensagem,
                    Aberto = true,
                    UsuarioId = matriculaMestre.AprendizId // Define o aprendiz como destinatário da notificação
                };

                _context.Notificacao.Add(notificacao);

                // Deleta a MatriculaMestre do contexto
                _context.MatriculaMestre.Remove(matriculaMestre);
                await _context.SaveChangesAsync();

                // Retorna a view do Index
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Tratar exceções, se necessário
                Console.WriteLine($"Erro ao finalizar matrícula: {ex.Message}");
                return StatusCode(500, "Erro ao finalizar matrícula.");
            }
        }


    }
}
