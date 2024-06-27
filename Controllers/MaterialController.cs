using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inveni.Models;
using Inveni.Persistence;
using Inveni.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Text;

namespace Inveni.Controllers {
    public class MaterialController : Controller {
        private readonly Contexto _context;
        private readonly IWebHostEnvironment _env;

        public MaterialController(Contexto context, IWebHostEnvironment env) {
            _context = context;
            _env = env;
        }

        [HttpGet]
        [Authorize(Policy = "Mestre")]
        public async Task<IActionResult> MaterialEnviadoHistorico() {
            var mestreId = Convert.ToInt32(User.Identity.Name);

            var materialEnviadosHistorico = await _context.MaterialEnviadoHistorico
                .Include(meh => meh.Material)
                .Include(meh => meh.Aprendiz)
                .Where(meh => meh.MestreId == mestreId)
                .OrderBy(meh => meh.Material.NomeArquivo)
                .ThenBy(meh => meh.Aprendiz.Nome)
                .ToListAsync();

            // Retornar os dados para a view MaterialEnviadoHistorico
            return View(materialEnviadosHistorico);
        }


        [HttpPost]
        [Authorize(Policy = "Mestre")]
        public IActionResult DeleteMaterialEnviadoHistorico(int id) {
            var registros = _context.MaterialEnviadoHistorico.Where(meh => meh.Id == id);
            _context.MaterialEnviadoHistorico.RemoveRange(registros);
            _context.SaveChanges();

            return Ok();
        }




        // GET: Material
        [Authorize(Policy = "Mestre")]
        public async Task<IActionResult> Index() {
            var contexto = _context.Material.Include(m => m.Mestre).Where(m => m.MestreId == Convert.ToInt32(User.Identity.Name) && m.Ativo);
            return View(await contexto.ToListAsync());
        }

        // GET: Material/Create
        [Authorize(Policy = "Mestre")]
        public IActionResult Create() {
            return View();
        }

        // POST: Material/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Mestre")]
        public async Task<IActionResult> Create([Bind("Id")] Material material, IFormFile arquivo) {
            if (arquivo != null && arquivo.Length > 0)
            {
                try {
                    material.NomeArquivo = Path.GetFileName(arquivo.FileName);
                    var userId = Convert.ToInt32(User.Identity.Name);
                    material.MestreId = userId;
                    material.Ativo = true;

                    var userIdFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "arquivos", userId.ToString());
                    if (!Directory.Exists(userIdFolder))
                    {
                        Directory.CreateDirectory(userIdFolder);
                    }

                    var fileName = Path.GetFileName(arquivo.FileName);
                    var filePath = Path.Combine(userIdFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await arquivo.CopyToAsync(fileStream);
                    }

                    material.CaminhoArquivo = $"/arquivos/{userId}/{fileName}";

                    _context.Add(material);
                    await _context.SaveChangesAsync();

                    // Retorna um JSON indicando sucesso
                    return Json(new { success = true, message = "Material carregado com sucesso!" });
                }
                catch (Exception ex) 
                {
                    return Json(new { success = true, message = "Não foi possível carregar o material, contate o suporte!" });
                }
               
            }
            else {
                return Json(new { success = false, message = "Material não carregado pelo usuário!" });
            }

          

            
        }

        // POST: Material/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // GET: Material/Delete/5
        [Authorize(Policy = "Mestre")]
        public async Task<IActionResult> Delete(string? id) {
            int encodeId = Funcoes.DecodeId(id);
            if (id == null || _context.Material == null)
            {
                return NotFound();
            }

            var material = await _context.Material
                .Include(m => m.Mestre)
                .FirstOrDefaultAsync(m => m.Id == encodeId && m.Ativo);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // POST: Material/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string? id) {
            int decodeId = Funcoes.DecodeId(id);
            if (_context.Material == null)
            {
                return Json(new { success = false, message = "Entity set 'Contexto.Material' is null." });
            }
            var material = await _context.Material.FindAsync(decodeId);
            if (material != null)
            {
                material.Ativo = false; // Atualiza o campo Ativo para false
                _context.Material.Update(material); // Atualiza o contexto com a nova alteração
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Material excluído com sucesso." });
            }

            return Json(new { success = false, message = "Material não encontrado." });
        }



        private bool MaterialExists(int id) {
            return (_context.Material?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //teste
        [HttpGet]
        [Authorize(Policy = "Mestre")]
        public IActionResult CompartilharMaterial(int id) {
            // Obter o material pelo ID
            var material = _context.Material.Find(id);

            if (material == null)
            {
                return NotFound();
            }

            int mestreId = Convert.ToInt32(User.Identity.Name);

            // Consulta as temáticas do mestre ordenadas alfabeticamente
            var tematicasMestre =  _context.TematicaMestre
             .Include(tm => tm.Tematica)
             .Include(tm => tm.MatriculaMestre)
                 .ThenInclude(mm => mm.Aprendiz)
             .Where(tm => tm.UsuarioId == mestreId)
             .OrderBy(tm => tm.Tematica.Descricao)
             .ToList();

            // Segundo bloco: Ordena as matrículas dentro de cada temática
            foreach (var tematica in tematicasMestre)
            {
                tematica.MatriculaMestre = tematica.MatriculaMestre.Where(mm => mm.Status == MatriculaStatus.Matriculado)
                    .OrderBy(mm => mm.Aprendiz.Nome)
                    .ToList();
            }


            // Criar um modelo para a exibição no modal
            var viewModel = new CompartilharMaterialViewModel
            {
                MaterialId = material.Id,
                TematicaMestre = tematicasMestre,
                MaterialNome = material.NomeArquivo
            };

            return PartialView("_CompartilharMaterialModal", viewModel);
        }



        /*[HttpGet]
        [Authorize(Policy = "Mestre")]
        public IActionResult CompartilharMaterial(int id) {
            // Obter o material pelo ID
            var material = _context.Material.Find(id);

            if (material == null)
            {
                return NotFound();
            }

            // Obter todos os aprendizes do mestre
            var aprendizes = _context.MatriculaMestre
            .Where(mm => mm.MestreId == material.MestreId)
            .Select(mm => mm.Aprendiz)
            .OrderBy(aprendiz => aprendiz.Nome)
            .ToList();



            // Criar um modelo para a exibição no modal
            var viewModel = new CompartilharMaterialViewModel
            {
                MaterialId = material.Id,
                Aprendizes = aprendizes,
                Material = material
            };

            return PartialView("_CompartilharMaterialModal", viewModel);
        }*/

        [HttpPost]
        [Authorize(Policy = "Mestre")]
        public IActionResult CompartilharMaterial(int id, List<int> matriculasMestreList) {
            // Obter o material pelo ID
            var material = _context.Material.Find(id);

            if (material == null)
            {
                return Json(new { success = false, message = "Material não encontrado." });
            }

            if (matriculasMestreList != null && matriculasMestreList.Any())
            {
                foreach (var matriculamestre in matriculasMestreList)
                {
                    var matriculaMestre = _context.MatriculaMestre
                        .Include(mm => mm.Mestre)
                        .Include(mm => mm.TematicaMestre)
                            .ThenInclude(tm => tm.Tematica)
                        .Where(mm => mm.Id == matriculamestre)
                        .OrderByDescending(mm => mm.Id)
                        .FirstOrDefault();

                    DateTime dataEnvio = DateTime.Now;

                    if (matriculaMestre != null)
                    {
                        var materialMatriculaMestre = new MaterialMatriculaMestre
                        {
                            MaterialId = material.Id,
                            MatriculaMestreId = matriculaMestre.Id,
                            AtivoAprendiz = true,
                            DataEnviado = dataEnvio
                        };

                        var materialHistoricoEnviado = new MaterialEnviadoHistorico
                        {
                            MaterialId = material.Id,
                            AprendizId = matriculaMestre.AprendizId,
                            MestreId = material.MestreId,
                            DataEnviado = dataEnvio
                        };

                        var mensagem = $"Material enviado pelo(a) Mestre {matriculaMestre.Mestre.Nome} - {matriculaMestre.TematicaMestre.Tematica.Descricao}: {material.NomeArquivo}";

                        var notificacao = new Notificacao
                        {
                            Descricao = mensagem,
                            Aberto = true,
                            UsuarioId = matriculaMestre.AprendizId
                        };

                        _context.Notificacao.Add(notificacao);
                        _context.MaterialMatriculaMestre.Add(materialMatriculaMestre);
                        _context.MaterialEnviadoHistorico.Add(materialHistoricoEnviado);
                    }
                }

                _context.SaveChanges();
            }

            return Json(new { success = true, message = "Material compartilhado com sucesso." });
        }

        [Authorize(Policy = "Aprendiz")]
        public IActionResult MateriaisEnviados() {
            // Obter o ID do aprendiz da sessão (substitua isso pela lógica real)
            var aprendizId = Convert.ToInt32(User.Identity.Name);

            var matriculas = _context.MatriculaMestre
                 .Where(mm => mm.AprendizId == aprendizId)
                 .SelectMany(mm => mm.MaterialMatriculaMestre)
                 .Where(mmm => mmm.AtivoAprendiz) // Verifica se AtivoAprendiz é verdadeiro
                 .Include(mmm => mmm.Material)
                     .ThenInclude(m => m.Mestre)
                 .OrderBy(mmm => mmm.DataEnviado)
                 .ToList();




            // Criar um modelo para a exibição na view
            var viewModel = new MateriaisEnviadosViewModel
            {
                Matriculas = matriculas
            };

            return View(viewModel);
        }
        [Authorize(Policy = "Aprendiz")]
        public async Task<IActionResult> DeleteMaterialAprendiz(int id) {
            var materialMatriculaMestre = await _context.MaterialMatriculaMestre.FindAsync(id);

            if (materialMatriculaMestre != null)
            {
                _context.MaterialMatriculaMestre.Remove(materialMatriculaMestre);
                await _context.SaveChangesAsync();
                return Ok(); // Retornar OK para indicar sucesso
            }

            return NotFound(); // Retornar NotFound caso não encontre o registro
        }


        [Authorize]
        public IActionResult Download(int materialId)
        {
            var material = _context.Material.Find(materialId);

            if (material == null || string.IsNullOrEmpty(material.CaminhoArquivo))
            {
                return NotFound();
            }

            // Obtenha o caminho completo do arquivo físico no servidor
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", material.CaminhoArquivo.TrimStart('/'));

            // Verifique se o arquivo existe no caminho fornecido
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Arquivo não encontrado
            }

            // Obtenha o conteúdo do arquivo como bytes
            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            // Use FileExtensionContentTypeProvider para obter o tipo MIME com base na extensão do arquivo
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            if (!contentTypeProvider.TryGetContentType(filePath, out var contentType))
            {
                // Se não for possível determinar o tipo MIME, use o padrão "application/octet-stream"
                contentType = "application/octet-stream";
            }

            // Retorne o arquivo para download com o nome correto do arquivo
            return File(fileBytes, contentType, material.NomeArquivo);
        }





    }
}
