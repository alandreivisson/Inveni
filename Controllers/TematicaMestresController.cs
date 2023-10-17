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
    public class TematicaMestresController : Controller
    {
        private readonly Contexto _context;

        public TematicaMestresController(Contexto context)
        {
            _context = context;
        }

        // GET: TematicaMestres
        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.Identity.Name);

            var contexto = _context.TematicaMestre
                .Include(t => t.Tematica)
                .ThenInclude(t => t.Categoria) // Inclua a Categoria
                .Include(t => t.Usuario)
                .Include(t => t.Modelo)
                .Where(t => t.Usuario.Id == userId)
                .OrderBy(t => t.Tematica.Descricao);

            return View(await contexto.ToListAsync());
        }

        // GET: TematicaMestres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TematicaMestre == null)
            {
                return NotFound();
            }

            var tematicaMestre = await _context.TematicaMestre
                .Include(t => t.Tematica)
                .Include(t => t.Usuario)
                .Include(t => t.Modelo)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (tematicaMestre == null)
            {
                return NotFound();
            }

            return View(tematicaMestre);
        }

        // GET: TematicaMestres/Create
        public IActionResult Create()
        {
            ViewData["TematicaId"] = new SelectList(_context.Tematica, "Id", "Descricao");
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Nome");
            ViewData["ModeloId"] = new SelectList(_context.Modelo, "Id", "Descricao");
            return View();
        }

        // POST: TematicaMestres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Biografia,TematicaId,UsuarioId,ModeloId,Ativo")] TematicaMestre tematicaMestre)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.Name;
                tematicaMestre.UsuarioId = int.Parse(userId);
                _context.Add(tematicaMestre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TematicaId"] = new SelectList(_context.Tematica, "Id", "Descricao", tematicaMestre.TematicaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Nome", tematicaMestre.UsuarioId);
            ViewData["ModeloId"] = new SelectList(_context.Modelo, "Id", "Descricao", tematicaMestre.ModeloId);
            return View(tematicaMestre);
        }

        // GET: TematicaMestres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tematicaMestre = await _context.TematicaMestre.FindAsync(id);
            if (tematicaMestre == null)
            {
                return NotFound();
            }

            ViewData["TematicaId"] = new SelectList(_context.Tematica, "Id", "Id", tematicaMestre.TematicaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Id", tematicaMestre.UsuarioId);
            ViewData["ModeloId"] = new SelectList(_context.Modelo, "Id", "Descricao", tematicaMestre.ModeloId);
            return View(tematicaMestre);
        }

        // POST: TematicaMestres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Biografia,TematicaId,UsuarioId,ModeloId,Ativo")] TematicaMestre tematicaMestre)
        {
            if (id != tematicaMestre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tematicaMestre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TematicaMestreExists(tematicaMestre.Id))
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
            ViewData["TematicaId"] = new SelectList(_context.Tematica, "Id", "Id", tematicaMestre.TematicaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Id", tematicaMestre.UsuarioId);
            ViewData["ModeloId"] = new SelectList(_context.Modelo, "Id", "Descricao", tematicaMestre.ModeloId);
            return View(tematicaMestre);
        }

        // GET: TematicaMestres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TematicaMestre == null)
            {
                return NotFound();
            }

            var tematicaMestre = await _context.TematicaMestre
                .Include(t => t.Tematica)
                .Include(t => t.Usuario)
                .Include(t => t.Modelo)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (tematicaMestre == null)
            {
                return NotFound();
            }

            return View(tematicaMestre);
        }

        // POST: TematicaMestres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TematicaMestre == null)
            {
                return Problem("Entity set 'Contexto.TematicaMestre'  is null.");
            }
            var tematicaMestre = await _context.TematicaMestre.FindAsync(id);
            if (tematicaMestre != null)
            {
                _context.TematicaMestre.Remove(tematicaMestre);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TematicaMestreExists(int id)
        {
          return (_context.TematicaMestre?.Any(e => e.UsuarioId == id)).GetValueOrDefault();
        }

        [HttpPost]
        public JsonResult ValidaCadastro(int tematicaId)
        {
            int usuarioId = int.Parse(User.Identity.Name);
            TematicaMestre tem = _context.TematicaMestre.Where(c => c.UsuarioId == usuarioId && c.TematicaId == tematicaId).ToList().FirstOrDefault();
            if (tem != null)
            {
                return Json("s");
            }
            else
            {
                return Json("n");
            }
        }
    }
}
