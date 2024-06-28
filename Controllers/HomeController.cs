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
        public async Task<IActionResult> Filtrar(FiltrarViewModel filtro, string searchTerm, bool? favoritos, bool?matriculado, bool? pendente) {
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

            // Filtrar favoritos, se necessário
            if (User.Identity.IsAuthenticated && User.Claims.First(c => c.Type == "Permissoes").Value == "3") {
                if (favoritos == null) {
                    favoritos = false;
                }
                if ((bool)favoritos)
                {
                    var idUsuario = User.FindFirst(ClaimTypes.Name)?.Value;
                    var idUsuarioInt = Convert.ToInt32(idUsuario);
                    var favoritosIds = await _context.Favoritado
                        .Where(f => f.AprendizId == idUsuarioInt)
                        .Select(f => f.TematicasMestreId)
                        .ToListAsync();

                    contexto = contexto.Where(t => favoritosIds.Contains(t.Id));
                }
                if (matriculado == null) {
                    matriculado = false;
                }
                if ((bool)matriculado) 
                {
                    var idUsuario = User.FindFirst(ClaimTypes.Name)?.Value;
                    var idUsuarioInt = Convert.ToInt32(idUsuario);
                    var matriculadoIds = await _context.MatriculaMestre
                        .Where(m => m.AprendizId == idUsuarioInt && m.Status == MatriculaStatus.Matriculado)
                        .Select(m => m.TematicaMestreId)
                        .ToListAsync();

                    contexto = contexto.Where(t => matriculadoIds.Contains(t.Id));
                }
                if (pendente == null)
                {
                    pendente = false;
                }
                if ((bool)pendente)
                {
                    var idUsuario = User.FindFirst(ClaimTypes.Name)?.Value;
                    var idUsuarioInt = Convert.ToInt32(idUsuario);
                    var matriculadoIds = await _context.MatriculaMestre
                        .Where(m => m.AprendizId == idUsuarioInt && m.Status == MatriculaStatus.Pendente)
                        .Select(m => m.TematicaMestreId)
                        .ToListAsync();

                    contexto = contexto.Where(t => matriculadoIds.Contains(t.Id));
                }

            }
           

            contexto = contexto.OrderBy(t => t.Tematica.Descricao);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                ViewBag.SearchTerm = searchTerm;
            }
            
            ViewBag.Categorias = new SelectList(await _context.Categoria.OrderBy(c => c.Descricao).ToListAsync(), "Id", "Descricao");
            ViewBag.Categorias = AddDefaultItem(ViewBag.Categorias, "Todas as Categorias");
            ViewBag.Tematicas = new SelectList(await _context.Tematica.OrderBy(t => t.Descricao).ToListAsync(), "Id", "Descricao");
            ViewBag.Tematicas = AddDefaultItem(ViewBag.Tematicas, "Todas as Temáticas");
            ViewBag.Modelos = new SelectList(await _context.Modelo.OrderBy(m => m.Descricao).ToListAsync(), "Id", "Descricao");
            ViewBag.Modelos = AddDefaultItem(ViewBag.Modelos, "Todos os Modelos");

            return View("Index", await contexto.ToListAsync());
        }


        //[HttpPost]
        //public async Task<IActionResult> Filtrar(FiltrarViewModel filtro, string searchTerm) {
        //    var contexto = _context.TematicaMestre
        //        .Include(t => t.Tematica)
        //        .ThenInclude(t => t.Categoria) // Inclua a Categoria
        //        .Include(t => t.Usuario)
        //        .Include(t => t.Modelo)
        //        .Where(t => t.Ativo);

        //    // Aplicar filtro de palavra-chave
        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        contexto = contexto.Where(t =>
        //            t.Usuario.Nome.Contains(searchTerm) ||
        //            t.Tematica.Descricao.Contains(searchTerm) ||
        //            t.Modelo.Descricao.Contains(searchTerm) ||
        //            t.Tematica.Categoria.Descricao.Contains(searchTerm));
        //    }

        //    if (filtro.CategoriaId.HasValue)
        //    {
        //        contexto = contexto.Where(t => t.Tematica.CategoriaId == filtro.CategoriaId);
        //    }

        //    if (filtro.TematicaId.HasValue)
        //    {
        //        contexto = contexto.Where(t => t.TematicaId == filtro.TematicaId);
        //    }

        //    if (filtro.ModeloId.HasValue)
        //    {
        //        contexto = contexto.Where(t => t.ModeloId == filtro.ModeloId);
        //    }

        //    contexto = contexto.OrderBy(t => t.Tematica.Descricao);

        //    ViewBag.Categorias = new SelectList(await _context.Categoria.OrderBy(c => c.Descricao).ToListAsync(), "Id", "Descricao");
        //    ViewBag.Categorias = AddDefaultItem(ViewBag.Categorias, "Todas as Categorias");
        //    ViewBag.Tematicas = new SelectList(await _context.Tematica.OrderBy(t => t.Descricao).ToListAsync(), "Id", "Descricao");
        //    ViewBag.Tematicas = AddDefaultItem(ViewBag.Tematicas, "Todas as Temáticas");
        //    ViewBag.Modelos = new SelectList(await _context.Modelo.OrderBy(m => m.Descricao).ToListAsync(), "Id", "Descricao");
        //    ViewBag.Modelos = AddDefaultItem(ViewBag.Modelos, "Todos os Modelos");

        //    return View("Index", await contexto.ToListAsync());
        //}
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
      .FirstOrDefault(t => t.Id == id && t.Ativo);

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
        [Authorize(Policy = "Aprendiz")]
        public async Task<IActionResult> SolicitarMatricula(int idAprendiz, int idTematicaMestre) {

            var tematicaMestre = await _context.TematicaMestre
            .Include(t => t.Tematica)
                .ThenInclude(t => t.Categoria)
            .Include(t => t.Usuario)
            .Include(t => t.Modelo)
            .FirstOrDefaultAsync(tm => tm.Id == idTematicaMestre && tm.Ativo);

            var matriculaMestre = new MatriculaMestre
            {
                MestreId = tematicaMestre.UsuarioId,
                AprendizId = idAprendiz,
                TematicaMestreId = tematicaMestre.Id,
                Status = MatriculaStatus.Pendente
            };

            _context.MatriculaMestre.Add(matriculaMestre);
            var usuario = _context.Usuario
                .FirstOrDefault(m => m.Id == idAprendiz);
            var mensagem = $"Matricula solicitada - Usuário(a): {usuario.Nome} - Temática: {tematicaMestre.Tematica.Descricao}";

            var notificacao = new Notificacao
            {
                Descricao = mensagem,
                Aberto = true,
                UsuarioId = matriculaMestre.MestreId // Define o aprendiz como destinatário da notificação
            };

            _context.Notificacao.Add(notificacao);

            await _context.SaveChangesAsync();

            var matricula = _context.MatriculaMestre
                                    .FirstOrDefault(m => m.TematicaMestreId == idTematicaMestre && m.AprendizId == idAprendiz);
            if (matricula != null)
                {
                    return Json(new { status = 1 }); // Retorne o novo status
                }

                return Json(new { status = 0 }); // Retorne um status de erro se não encontrado
        }
        [HttpPost]
        [Authorize(Policy = "Aprendiz")]
        public async Task<IActionResult> FavoritarTematicaMestre(int idAprendiz, int idTematicaMestre) {

           

            var favoritado = new Favoritado
            {
                AprendizId = idAprendiz,
                TematicasMestreId = idTematicaMestre
                // Define o aprendiz como destinatário da notificação
            };

            _context.Favoritado.Add(favoritado);

            await _context.SaveChangesAsync();

            var favoritadoVerifica = _context.Favoritado
                                             .FirstOrDefault(m => m.TematicasMestreId == idTematicaMestre && m.AprendizId == idAprendiz);
          
            if (favoritadoVerifica != null)
            {
                return Json(new { status = 0}); // Retorne o novo status
            }

            return Json(new { status = 1 }); // Retorne um status de erro se não encontrado
        }

        [HttpPost]
        [Authorize(Policy = "Aprendiz")]
        public async Task<IActionResult> DesfavoritarTematicaMestre(int idAprendiz, int idTematicaMestre) {

            var favoritado = _context.Favoritado
                                            .FirstOrDefault(m => m.TematicasMestreId == idTematicaMestre && m.AprendizId == idAprendiz);

            _context.Favoritado.Remove(favoritado);

            await _context.SaveChangesAsync();

            var favoritadoVerifica = _context.Favoritado
                                             .FirstOrDefault(m => m.TematicasMestreId == idTematicaMestre && m.AprendizId == idAprendiz);

            if (favoritadoVerifica != null)
            {
                return Json(new { status = 1 }); // Retorne o novo status
            }

            return Json(new { status = 0 }); // Retorne um status de erro se não encontrado
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
                   var favorito = aprendiz.Favoritos.FirstOrDefault(f => f.TematicasMestreId == temaId && f.AprendizId == aprendizId);

                    if (favoritado && favorito == null)
                    {
        //                // Se favoritado e não existir, cria um novo registro
                        aprendiz.Favoritos.Add(new Favorito { AprendizId = aprendizId, TematicasMestreId = temaId, Favoritado = true });
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
        [HttpPost]
        [Authorize(Policy = "Aprendiz")]
        public ActionResult VerificarFavoritado(int id, int idAprendiz) {
            var isUserAuthenticated = User.Identity.IsAuthenticated.ToString().ToLower();
            if (!string.IsNullOrEmpty(isUserAuthenticated))
            {
                var favoritado = _context.Favoritado
                                                .FirstOrDefault(m => m.TematicasMestreId == id && m.AprendizId == idAprendiz);
                if (favoritado != null) {
                    return Json(new
                    {
                        status = 0
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = 1
                    });
                }
            }
            else
            {
                // Usuário não autenticado, lidar com isso de acordo com a sua lógica de negócios
                return Json(null);
            }
        }
            [HttpPost]
        [Authorize(Policy = "Aprendiz")]
        public ActionResult VerificarMatricula(int id, int idAprendiz) {
            var isUserAuthenticated = User.Identity.IsAuthenticated.ToString().ToLower();
            if (!string.IsNullOrEmpty(isUserAuthenticated))
            {
                var matricula = _context.MatriculaMestre
                                        .FirstOrDefault(m => m.TematicaMestreId == id && m.AprendizId == idAprendiz);

                if (matricula != null)
                {
                    
                    var result = matricula.Status;
                    if (result == MatriculaStatus.Matriculado)
                    {
                        return Json(new
                        {
                            status = 0
                        });
                    }
                    else {
                        return Json(new
                        {
                            status = 1
                        });
                    }
                }
                else
                {
                    // Se não existir, devolve 3
                    return Json(new
                    {
                        status = 3
                    });
                }
            }
            else
            {
                // Usuário não autenticado, lidar com isso de acordo com a sua lógica de negócios
                return Json(null);
            }
        }
    }


}