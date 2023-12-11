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

namespace Inveni.Controllers
{
    [Authorize(Policy = "Mestre")]
    public class MatriculaMestreController : Controller
    {
        private readonly Contexto _context;

        public MatriculaMestreController(Contexto context)
        {
            _context = context;
        }

        // GET: MatriculaMestre
        public async Task<IActionResult> Index()
        {
            var matriculasMestre = _context.MatriculaMestre
       .Include(m => m.Aprendiz)
       .Include(m => m.Mestre)
       .Where(m => m.MestreId == Convert.ToInt32(User.Identity.Name))
       .OrderBy(m => m.Aprendiz.Nome)
       .ToList();

            return View(matriculasMestre);
        }
        // GET: MatriculaMestre/Create
        public IActionResult Create()
        {

            var usuariosNaoCadastrados = GetUsuariosNaoCadastrados(Convert.ToInt32(User.Identity.Name));
            ViewBag.AprendizId = usuariosNaoCadastrados;

            return View();
        }

        // POST: MatriculaMestre/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AprendizId")] MatriculaMestre matriculaMestre) {
            if (ModelState.IsValid)
            {
                matriculaMestre.Status = MatriculaStatus.Matriculado;
                matriculaMestre.MestreId = Convert.ToInt32(User.Identity.Name);



                _context.Add(matriculaMestre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Se o ModelState for inválido, recupere a SelectList original da ViewBag
            var usuariosNaoCadastrados = GetUsuariosNaoCadastrados(Convert.ToInt32(User.Identity.Name));
            ViewBag.AprendizId = usuariosNaoCadastrados;

            // Retorne a view com o modelo que falhou a validação
            return View(matriculaMestre);
        }


        // GET: MatriculaMestre/Delete/5
        public async Task<IActionResult> Delete(string? id) {
            int encodeId = Funcoes.DecodeId(id);
            if (id == null || _context.MatriculaMestre == null)
            {
                return NotFound();
            }

            var matriculaMestre = await _context.MatriculaMestre
                .Include(m => m.Aprendiz)
                .Include(m => m.Mestre)
                .FirstOrDefaultAsync(m => m.Id == encodeId);
            if (matriculaMestre == null)
            {
                return NotFound();
            }

            return View(matriculaMestre);
        }

        // POST: MatriculaMestre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string? id) {
            int decodeId = Funcoes.DecodeId(id);
            if (_context.MatriculaMestre == null)
            {
                return Problem("Entity set 'Contexto.MatriculaMestre'  is null.");
            }
            var matriculaMestre = await _context.MatriculaMestre.FindAsync(decodeId);
            if (matriculaMestre != null)
            {
                _context.MatriculaMestre.Remove(matriculaMestre);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatriculaMestreExists(int id)
        {
          return (_context.MatriculaMestre?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public SelectList GetUsuariosNaoCadastrados(int idMestre) {
            var usuariosNaoCadastrados = _context.Usuario
                .Include(u => u.UsuarioPerfil)
                .Where(u => u.UsuarioPerfil.Any(up => up.PerfilId == 3) && !_context.MatriculaMestre
                    .Any(m => m.MestreId == idMestre && m.AprendizId == u.Id))
                .ToList();

            // Crie e retorne o SelectList diretamente
            return new SelectList(usuariosNaoCadastrados, "Id", "Email");
        }
    }
}
