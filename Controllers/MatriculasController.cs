using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inveni.Models;
using Inveni.Persistence;

namespace Inveni.Controllers {
    public class MatriculasController : Controller {
        private readonly Contexto _context;

        public MatriculasController(Contexto context) {
            _context = context;
        }

        // GET: Matriculas
        public async Task<IActionResult> Index() {


            var contexto = _context.Matricula
            .Include(m => m.Aprendiz)
            .Include(m => m.TematicaMestre)
                .ThenInclude(tm => tm.Usuario)
            .Include(m => m.TematicaMestre)
                .ThenInclude(tm => tm.Tematica)
            .Where(m => m.TematicaMestre.UsuarioId == Convert.ToInt32(User.Identity.Name))
            .OrderBy(m => m.Aprendiz.Nome);

            return View(await contexto.ToListAsync());
        }

        // GET: Matriculas/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null || _context.Matricula == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matricula
                .Include(m => m.Aprendiz)
                .Include(m => m.TematicaMestre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (matricula == null)
            {
                return NotFound();
            }

            return View(matricula);
        }

        // GET: Matriculas/Create
        //public IActionResult Create()
        //{
        //    ViewData["AprendizId"] = new SelectList(_context.Usuario, "Id", "Id");
        //    ViewData["TematicaMestreId"] = new SelectList(_context.TematicaMestre, "Id", "Id");
        //    return View();
        //}
        public IActionResult Create() {
            var tematicasMestre = _context.TematicaMestre.Include(tm => tm.Tematica).Where(tm => tm.UsuarioId == Convert.ToInt32(User.Identity.Name)).ToList();
            ViewBag.TematicaMestreId = new SelectList(tematicasMestre, "Id", "Tematica.Descricao");

            // Certifique-se de ter a instância de Matricula para acessar a propriedade TematicaMestreId
            var matricula = new Matricula(); // Substitua isso pela sua lógica para obter a instância de Matricula

            // Faça uma chamada AJAX para obter os usuários não cadastrados
            var usuariosNaoCadastrados = GetUsuariosNaoCadastrados(matricula.TematicaMestreId);
            ViewBag.AprendizId = new SelectList((System.Collections.IEnumerable)usuariosNaoCadastrados, "Id", "Email");

            return View();
        }



        // POST: Matriculas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AprendizId,TematicaMestreId,Status")] Matricula matricula) {
            if (ModelState.IsValid)
            {
                // Adicione lógica para buscar usuários que não estão cadastrados para a temática do mestre
                var usuariosNaoCadastrados = _context.Usuario
                    .Where(u => !_context.Matricula.Any(m => m.TematicaMestreId == matricula.TematicaMestreId && m.AprendizId == u.Id))
                    .ToList();

                ViewBag.AprendizId = new SelectList(usuariosNaoCadastrados, "Id", "Email", matricula.AprendizId);
                matricula.Status = MatriculaStatus.Matriculado;

                _context.Add(matricula);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var tematicasMestre = _context.TematicaMestre.Include(tm => tm.Tematica).Where(tm => tm.UsuarioId == Convert.ToInt32(User.Identity.Name)).ToList();
            ViewBag.TematicaMestreId = new SelectList(tematicasMestre, "Id", "Tematica.Descricao");

            return View(matricula);
        }


        // GET: Matriculas/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null || _context.Matricula == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matricula.FindAsync(id);
            if (matricula == null)
            {
                return NotFound();
            }
            ViewData["AprendizId"] = new SelectList(_context.Usuario, "Id", "Id", matricula.AprendizId);
            ViewData["TematicaMestreId"] = new SelectList(_context.TematicaMestre, "Id", "Id", matricula.TematicaMestreId);
            return View(matricula);
        }

        // POST: Matriculas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AprendizId,TematicaMestreId,Status")] Matricula matricula) {
            if (id != matricula.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(matricula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatriculaExists(matricula.Id))
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
            ViewData["AprendizId"] = new SelectList(_context.Usuario, "Id", "Id", matricula.AprendizId);
            ViewData["TematicaMestreId"] = new SelectList(_context.TematicaMestre, "Id", "Id", matricula.TematicaMestreId);
            return View(matricula);
        }

        // GET: Matriculas/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null || _context.Matricula == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matricula
                .Include(m => m.Aprendiz)
                .Include(m => m.TematicaMestre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (matricula == null)
            {
                return NotFound();
            }

            return View(matricula);
        }

        // POST: Matriculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            if (_context.Matricula == null)
            {
                return Problem("Entity set 'Contexto.Matriculas'  is null.");
            }
            var matricula = await _context.Matricula.FindAsync(id);
            if (matricula != null)
            {
                _context.Matricula.Remove(matricula);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatriculaExists(int id) {
            return (_context.Matricula?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpGet]
        public JsonResult GetUsuariosNaoCadastrados(int tematicaMestreId) {
            var usuariosNaoCadastrados = _context.Usuario
                .Include(u => u.UsuarioPerfil)  // Garante que a propriedade UsuarioPerfil seja carregada
                .Where(u => u.UsuarioPerfil.Any(up => up.PerfilId == 3) && !_context.Matricula
                    .Any(m => m.TematicaMestreId == tematicaMestreId && m.AprendizId == u.Id))
                .ToList();

            return Json(usuariosNaoCadastrados);
        }

    }
}
