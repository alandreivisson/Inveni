using Inveni.Models;
using Inveni.Persistence;
using Inveni.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Inveni.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly Contexto _context;

        public HomeController(ILogger<HomeController> logger, Contexto context) {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index(int? categoriaId, int? tematicaId, int? modeloId) {
            ViewBag.Categorias = new SelectList(await _context.Categoria.OrderBy(c => c.Descricao).ToListAsync(), "Id", "Descricao");
            ViewBag.Categorias = AddDefaultItem(ViewBag.Categorias, "Todas as Categorias");
            ViewBag.Tematicas = new SelectList(await _context.Tematica.OrderBy(t => t.Descricao).ToListAsync(), "Id", "Descricao");
            ViewBag.Tematicas = AddDefaultItem(ViewBag.Tematicas, "Todas as Temáticas");
            ViewBag.Modelos = new SelectList(await _context.Modelo.OrderBy(m => m.Descricao).ToListAsync(), "Id", "Descricao");
            ViewBag.Modelos = AddDefaultItem(ViewBag.Modelos, "Todos os Modelos");

            var contexto = _context.TematicaMestre
                .Include(t => t.Tematica)
                .ThenInclude(t => t.Categoria) // Inclua a Categoria
                .Include(t => t.Usuario)
                .Include(t => t.Modelo)
                .Where(t => t.Ativo);

            if (categoriaId.HasValue)
            {
                contexto = contexto.Where(t => t.Tematica.CategoriaId == categoriaId);
            }

            if (tematicaId.HasValue)
            {
                contexto = contexto.Where(t => t.TematicaId == tematicaId);
            }

            if (modeloId.HasValue)
            {
                contexto = contexto.Where(t => t.ModeloId == modeloId);
            }

            contexto = contexto.OrderBy(t => t.Tematica.Descricao);

            return View(await contexto.ToListAsync());
        }


        [Authorize]
        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Sair() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Filtrar(FiltrarViewModel filtro, string searchTerm) {
            var contexto = _context.TematicaMestre
                .Include(t => t.Tematica)
                .ThenInclude(t => t.Categoria) // Inclua a Categoria
                .Include(t => t.Usuario)
                .Include(t => t.Modelo)
                .Where(t => t.Ativo);

            // Aplicar filtro de palavra-chave
            if (!string.IsNullOrEmpty(searchTerm))
            {
                contexto = contexto.Where(t =>
                    t.Usuario.Nome.Contains(searchTerm) ||
                    t.Tematica.Descricao.Contains(searchTerm) ||
                    t.Modelo.Descricao.Contains(searchTerm) ||
                    t.Tematica.Categoria.Descricao.Contains(searchTerm));
            }

            if (filtro.CategoriaId.HasValue)
            {
                contexto = contexto.Where(t => t.Tematica.CategoriaId == filtro.CategoriaId);
            }

            if (filtro.TematicaId.HasValue)
            {
                contexto = contexto.Where(t => t.TematicaId == filtro.TematicaId);
            }

            if (filtro.ModeloId.HasValue)
            {
                contexto = contexto.Where(t => t.ModeloId == filtro.ModeloId);
            }

            contexto = contexto.OrderBy(t => t.Tematica.Descricao);

            ViewBag.Categorias = new SelectList(await _context.Categoria.OrderBy(c => c.Descricao).ToListAsync(), "Id", "Descricao");
            ViewBag.Categorias = AddDefaultItem(ViewBag.Categorias, "Todas as Categorias");
            ViewBag.Tematicas = new SelectList(await _context.Tematica.OrderBy(t => t.Descricao).ToListAsync(), "Id", "Descricao");
            ViewBag.Tematicas = AddDefaultItem(ViewBag.Tematicas, "Todas as Temáticas");
            ViewBag.Modelos = new SelectList(await _context.Modelo.OrderBy(m => m.Descricao).ToListAsync(), "Id", "Descricao");
            ViewBag.Modelos = AddDefaultItem(ViewBag.Modelos, "Todos os Modelos");

            return View("Index", await contexto.ToListAsync());
        }
        public IActionResult Detalhes(int id) 
        {

            if (!User.Identity.IsAuthenticated)
            {
                // Redirecionar para a action "Acesso" da classe "Usuario"
                return RedirectToAction("Acesso", "Usuarios");
            }

            var detalhes = _context.TematicaMestre
        .Include(t => t.Tematica)
        .ThenInclude(t => t.Categoria)
        .Include(t => t.Usuario)
        .Include(t => t.Modelo)
        .Include(t => t.Matriculas) // Inclua as matrículas
        .FirstOrDefault(t => t.Id == id);

            return PartialView("_DetalhesModal", detalhes); // Crie uma PartialView para formatar os detalhes como desejar
        }
        private SelectList AddDefaultItem(SelectList originalList, string defaultText) {
            var items = new List<SelectListItem>
            {
                new SelectListItem { Value = null, Text = defaultText, Disabled = true, Selected = true }
            };

            items.AddRange(originalList);

            return new SelectList(items, "Value", "Text");
        }

        [HttpPost]
        public IActionResult SolicitarMatricula(int tematicaMestreId) {
            
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);
            var matricula = new Matricula
            {
                AprendizId = userId,
                TematicaMestreId = tematicaMestreId,
                Status = MatriculaStatus.Pendente
            };

            _context.Add(matricula);
            _context.SaveChanges();

            // Redirecione de volta para a página de detalhes
            return RedirectToAction("Detalhes", new { id = tematicaMestreId });
        }

        [HttpPost]
        public IActionResult FavoritarDesfavoritar(int temaId, int aprendizId, bool favoritado) {
            try
            {
                // Recupera o aprendiz
                var aprendiz = _context.Usuario.Include(a => a.Favoritos).FirstOrDefault(a => a.Id == aprendizId);

         // Recupera o tema
                var tema = _context.TematicaMestre.Find(temaId);

                if (aprendiz != null && tema != null)
                 {
                    // Verifica se já existe um registro de favorito para este tema e aprendiz
                   var favorito = aprendiz.Favoritos.FirstOrDefault(f => f.TematicaMestreId == temaId && f.AprendizId == aprendizId);

                    if (favoritado && favorito == null)
                    {
        //                // Se favoritado e não existir, cria um novo registro
                        aprendiz.Favoritos.Add(new Favorito { AprendizId = aprendizId, TematicaMestreId = temaId, Favoritado = true });
                    }
                    else if (!favoritado && favorito != null)
                    {
                        // Se desfavoritado e existir, remove o registro
                       aprendiz.Favoritos.Remove(favorito);
                    }

                    _context.SaveChanges();

                    return Json(new { success = true, favoritado = !favoritado });
                }

                return Json(new
                {
                    success = false,
                    message = "Aprendiz ou Tema não encontrados."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Erro ao favoritar/desfavoritar: {ex.Message}"
                });
            }
        }


    }


}