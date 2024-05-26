using Inveni.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using Inveni.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Inveni.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class HomeApiController : Controller {

        private readonly ILogger<HomeApiController> _logger;
        private readonly Contexto _context;

        public HomeApiController(ILogger<HomeApiController> logger, Contexto context) {
            _logger = logger;
            _context = context;
        }

        [HttpGet("IndexJson")]
        public async Task<IActionResult> GetIndexJson(int? categoriaId, int? tematicaId, int? modeloId) {
            var query = _context.TematicaMestre
                .Where(t => t.Ativo);

            if (categoriaId.HasValue)
            {
                query = query.Where(t => t.Tematica.CategoriaId == categoriaId);
            }

            if (tematicaId.HasValue)
            {
                query = query.Where(t => t.TematicaId == tematicaId);
            }

            if (modeloId.HasValue)
            {
                query = query.Where(t => t.ModeloId == modeloId);
            }

            var result = await query
                .Select(t => new
                {
                    t.Id,
                    TematicaDescricao = t.Tematica.Descricao,
                    UsuarioNome = t.Usuario.Nome,
                    UsuarioCaminhoFoto = t.Usuario.CaminhoFoto,
                    CategoriaDescricao = t.Tematica.Categoria.Descricao,
                    ModeloDescricao = t.Modelo.Descricao,
                    Biografia = t.Biografia,
                    // Adicione outras propriedades que você precisa aqui
                })
                .OrderBy(t => t.TematicaDescricao)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("FiltroJson")]
        public async Task<IActionResult> GetSelectListsFiltro() {
            var categorias = await _context.Categoria
                .OrderBy(c => c.Descricao)
                .Select(c => new { c.Id, c.Descricao })
                .ToListAsync();
            categorias.Insert(0, new { Id = 0, Descricao = "Todas as Categorias" });

            var tematicas = await _context.Tematica
                .OrderBy(t => t.Descricao)
                .Select(t => new { t.Id, t.Descricao })
                .ToListAsync();
            tematicas.Insert(0, new { Id = 0, Descricao = "Todas as Temáticas" });

            var modelos = await _context.Modelo
                .OrderBy(m => m.Descricao)
                .Select(m => new { m.Id, m.Descricao })
                .ToListAsync();
            modelos.Insert(0, new { Id = 0, Descricao = "Todos os Modelos" });

            var result = new
            {
                Categorias = categorias,
                Tematicas = tematicas,
                Modelos = modelos
            };

            return Json(result);
        }

        [HttpPost("FiltrarApiJson")]
        public async Task<IActionResult> FiltrarApiJson([FromBody] FiltrarApiViewModel filtro) {
            var query = _context.TematicaMestre
                .Include(t => t.Tematica)
                .ThenInclude(t => t.Categoria)
                .Include(t => t.Usuario)
                .Include(t => t.Modelo)
                .Where(t => t.Ativo);

            // Aplicar filtro de palavra-chave
            if (!string.IsNullOrEmpty(filtro.SearchTerm))
            {
                query = query.Where(t =>
                    t.Usuario.Nome.Contains(filtro.SearchTerm) ||
                    t.Tematica.Descricao.Contains(filtro.SearchTerm) ||
                    t.Modelo.Descricao.Contains(filtro.SearchTerm) ||
                    t.Tematica.Categoria.Descricao.Contains(filtro.SearchTerm));
            }

            // Aplicar filtros de CategoriaId, TematicaId e ModeloId
            if (filtro.CategoriaId.HasValue)
            {
                query = query.Where(t => t.Tematica.CategoriaId == filtro.CategoriaId);
            }

            if (filtro.TematicaId.HasValue)
            {
                query = query.Where(t => t.TematicaId == filtro.TematicaId);
            }

            if (filtro.ModeloId.HasValue)
            {
                query = query.Where(t => t.ModeloId == filtro.ModeloId);
            }

            var result = await query
                .Select(t => new
                {
                    t.Id,
                    TematicaDescricao = t.Tematica.Descricao,
                    UsuarioNome = t.Usuario.Nome,
                    UsuarioCaminhoFoto = t.Usuario.CaminhoFoto,
                    CategoriaDescricao = t.Tematica.Categoria.Descricao,
                    ModeloDescricao = t.Modelo.Descricao,
                    Biografia = t.Biografia,
                    // Adicione outras propriedades que você precisa aqui
                })
                .OrderBy(t => t.TematicaDescricao)
                .ToListAsync();

            return Ok(result);
        }


        [HttpPost("FiltrarJson")]
        public async Task<IActionResult> PostFiltrarJson(FiltrarViewModel filtro, string searchTerm) {
            var query = _context.TematicaMestre
                .Where(t => t.Ativo);

            // Aplicar filtro de palavra-chave
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t =>
                    t.Usuario.Nome.Contains(searchTerm) ||
                    t.Tematica.Descricao.Contains(searchTerm) ||
                    t.Modelo.Descricao.Contains(searchTerm) ||
                    t.Tematica.Categoria.Descricao.Contains(searchTerm));
            }

            if (filtro.CategoriaId.HasValue)
            {
                query = query.Where(t => t.Tematica.CategoriaId == filtro.CategoriaId);
            }

            if (filtro.TematicaId.HasValue)
            {
                query = query.Where(t => t.TematicaId == filtro.TematicaId);
            }

            if (filtro.ModeloId.HasValue)
            {
                query = query.Where(t => t.ModeloId == filtro.ModeloId);
            }

            query = query.OrderBy(t => t.Tematica.Descricao);

            var result = await query
              .Select(t => new
              {
                  t.Id,
                  TematicaDescricao = t.Tematica.Descricao,
                  UsuarioNome = t.Usuario.Nome,
                  UsuarioCaminhoFoto = t.Usuario.CaminhoFoto,
                  CategoriaDescricao = t.Tematica.Categoria.Descricao,
                  ModeloDescricao = t.Modelo.Descricao,
                  Biografia = t.Biografia,
                  // Adicione outras propriedades que você precisa aqui
              })
              .OrderBy(t => t.TematicaDescricao)
              .ToListAsync();

            return Ok(result);
        }

    }
}
