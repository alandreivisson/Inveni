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
