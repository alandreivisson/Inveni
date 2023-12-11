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
using System.Data;

namespace Inveni.Controllers
{
    [Authorize(Policy = "Administrador")]
    public class TematicasController : Controller
    {
        private readonly Contexto _context;

        public TematicasController(Contexto context)
        {
            _context = context;
        }

        // GET: Tematicas
        public async Task<IActionResult> Index()
        {
            var contexto = _context.Tematica.Include(t => t.Categoria);
            return View(await contexto.ToListAsync());
        }

        // GET: Tematicas/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Descricao");
            return View();
        }

        // POST: Tematicas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,CategoriaId")] Tematica tematica)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tematica);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Descricao", tematica.CategoriaId);
            return View(tematica);
        }

        // GET: Tematicas/Edit/5
        public async Task<IActionResult> Edit(string id) {
            int encodeId = Funcoes.DecodeId(id);
            if (encodeId == null || _context.Tematica == null)
            {
                return NotFound();
            }

            var tematica = await _context.Tematica.FindAsync(encodeId);
            if (tematica == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Descricao", tematica.CategoriaId);
            return View(tematica);
        }

        // POST: Tematicas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? id, [Bind("Id,Descricao,CategoriaId")] Tematica tematica)
        {
            int decodeId = Funcoes.DecodeId(id);
            if (decodeId != tematica.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tematica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TematicaExists(tematica.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Descricao", tematica.CategoriaId);
            return View(tematica);
        }

        // GET: Tematicas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tematica == null)
            {
                return NotFound();
            }

            var tematica = await _context.Tematica
                .Include(t => t.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tematica == null)
            {
                return NotFound();
            }

            return View(tematica);
        }

        // POST: Tematicas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tematica == null)
            {
                return Problem("Entity set 'Contexto.Tematica'  is null.");
            }
            var tematica = await _context.Tematica.FindAsync(id);
            if (tematica != null)
            {
                _context.Tematica.Remove(tematica);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TematicaExists(int id)
        {
          return (_context.Tematica?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
