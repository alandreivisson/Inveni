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
            var matriculas = await _context.TematicaMestre
             .Include(t => t.Matriculas)  // Inclui informações sobre as matrículas da temática
             .Include(t => t.Usuario)    // Inclui informações sobre os usuários
             .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == Convert.ToInt32(User.Identity.Name));
            if (matriculas == null)
            {
                return NotFound();
            }

            return View(matriculas);
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
            ViewBag.TematicaDescricao = _context.Tematica.FirstOrDefault(t => t.Id == tematicaMestre.TematicaId)?.Descricao;
            return View(tematicaMestre);
        }
        // GET: TematicaMestres/Edit/5
        // GET: TematicaMestres/Edit/5
        // GET: TematicaMestres/Edit/5
        public async Task<IActionResult> Edit(string id) {
            int encodeId = Funcoes.DecodeId(id);
            if (encodeId == null)
            {
                return NotFound();
            }

            var tematicaMestre = await _context.TematicaMestre.FindAsync(encodeId);
            if (tematicaMestre == null)
            {
                return NotFound();
            }

            ViewData["TematicaId"] = new SelectList(_context.Tematica, "Id", "Descricao", tematicaMestre.TematicaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Id", tematicaMestre.UsuarioId);
            ViewData["ModeloId"] = new SelectList(_context.Modelo, "Id", "Descricao", tematicaMestre.ModeloId);
            ViewData["TematicaDescricao"] = _context.Tematica.FirstOrDefault(t => t.Id == tematicaMestre.TematicaId)?.Descricao;

            // Redirecionar para a ação de edição com o novo formato de URL
            return View(tematicaMestre);
        }

        // POST: TematicaMestres/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? id, [Bind("Id,Biografia,TematicaId,UsuarioId,ModeloId,Ativo")] TematicaMestre tematicaMestre) {
            // Decodifique o ID
            int decodeId = Funcoes.DecodeId(id);
            tematicaMestre.Id = decodeId;
                       
            if (tematicaMestre.Id == decodeId)
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
            ViewData["TematicaId"] = new SelectList(_context.Tematica, "Id", "Descricao", tematicaMestre.TematicaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Id", tematicaMestre.UsuarioId);
            ViewData["ModeloId"] = new SelectList(_context.Modelo, "Id", "Descricao", tematicaMestre.ModeloId);
            ViewData["TematicaDescricao"] = _context.Tematica.FirstOrDefault(t => t.Id == tematicaMestre.TematicaId)?.Descricao;

            return View(tematicaMestre);
        }



        // GET: TematicaMestres/Delete/5
        public async Task<IActionResult> Delete(string? id) {
            int encodeId = Funcoes.DecodeId(id);
            if (encodeId == null)
            {
                return NotFound();
            }

            var tematicaMestre = await _context.TematicaMestre
                .Include(t => t.Tematica)
                .Include(t => t.Usuario)
                .Include(t => t.Modelo)
                .FirstOrDefaultAsync(m => m.Id == encodeId); // Use m.Id para comparar com o id passado
            if (tematicaMestre == null)
            {
                return NotFound();
            }

            return View(tematicaMestre);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string? id) {
            int decodeId = Funcoes.DecodeId(id);
            var tematicaMestre = await _context.TematicaMestre.FindAsync(decodeId);
            if (tematicaMestre != null)
            {
                _context.TematicaMestre.Remove(tematicaMestre);
                await _context.SaveChangesAsync();
            }
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
        [HttpPost]
        public async Task<IActionResult> MatriculasTematicas(int id) {
            var matriculas = await _context.TematicaMestre
         .Include(t => t.Matriculas)  // Inclui informações sobre as matrículas da temática
         .Include(t => t.Usuario)    // Inclui informações sobre os usuários
         .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == int.Parse(User.Identity.Name));

            if (matriculas == null)
            {
                return NotFound();
            }

            return View(matriculas);
        }
    }
}
