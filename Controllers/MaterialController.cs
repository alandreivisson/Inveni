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
        public IActionResult MaterialEnviadoHistorico() {
            var mestreId = Convert.ToInt32(User.Identity.Name);
            // Consulta para obter os dados de MaterialEnviadoHistorico com base no ID do mestre
            /*var materialEnviadoHistorico = _context.MaterialEnviadoHistorico
            .Where(meh => meh.MestreId == mestreId)
            .Include(meh => meh.Material)
            .Include(meh => meh.Aprendiz)
            .GroupBy(meh => new { meh.MaterialId, meh.DataEnviado })
            .Select(g => g.FirstOrDefault())
            .ToList();*/


            // Consulta para obter os dados de MaterialEnviadoHistorico com base no ID do mestre
            var materialEnviadoHistorico = _context.MaterialEnviadoHistorico
                .Where(meh => meh.MestreId == mestreId)
                .Include(meh => meh.Material)
                .Include(meh => meh.Aprendiz)
                .ToList()
                .GroupBy(meh => meh.DataEnviado.Date) // Agrupar por DataEnviado
                .ToList();



            // Retornar os dados para a view MaterialEnviadoHistorico
            return View(materialEnviadoHistorico);
        }

        [HttpPost]
        [Authorize(Policy = "Mestre")]
        public IActionResult DeleteMaterialEnviadoHistorico(int materialId) {
            var registros = _context.MaterialEnviadoHistorico.Where(meh => meh.MaterialId == materialId);
            _context.MaterialEnviadoHistorico.RemoveRange(registros);
            _context.SaveChanges();

            return Ok();
        }




        // GET: Material
        [Authorize(Policy = "Mestre")]
        public async Task<IActionResult> Index() {
            var contexto = _context.Material.Include(m => m.Mestre).Where(m => m.MestreId == Convert.ToInt32(User.Identity.Name));
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
                // Obter o nome do arquivo
                material.NomeArquivo = Path.GetFileName(arquivo.FileName);

                // Obter o ID do usuário da sessão
                // (Substitua isso com a lógica real para obter o ID do usuário da sessão)
                var userId = Convert.ToInt32(User.Identity.Name); // Exemplo: substitua isso com sua lógica real

                // Definir o MestreId
                material.MestreId = userId;

                // Criar diretório se não existir
                var userDirectory = Path.Combine(_env.ContentRootPath, "Arquivos", userId.ToString());
                if (!Directory.Exists(userDirectory))
                {
                    Directory.CreateDirectory(userDirectory);
                }

                // Definir o caminho do arquivo
                var filePath = Path.Combine(userDirectory, material.NomeArquivo);

                // Salvar o arquivo
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await arquivo.CopyToAsync(fileStream);
                }

                // Definir o CaminhoArquivo
                material.CaminhoArquivo = filePath;
            }

            _context.Add(material);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


            // return View(material);
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
                .FirstOrDefaultAsync(m => m.Id == encodeId);
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
                return Problem("Entity set 'Contexto.Material'  is null.");
            }
            var material = await _context.Material.FindAsync(decodeId);
            if (material != null)
            {
                _context.Material.Remove(material);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialExists(int id) {
            return (_context.Material?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpGet]
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
        }

        [HttpPost]
        [Authorize(Policy = "Mestre")]
        public IActionResult CompartilharMaterial(int id, List<int> AprendizesSelecionados) {
            // Obter o material pelo ID
            var material = _context.Material.Find(id);

            if (material == null)
            {
                return NotFound();
            }

            if (AprendizesSelecionados != null && AprendizesSelecionados.Any())
            {
                foreach (var aprendizId in AprendizesSelecionados)
                {
                    // Obter a matrícula do aprendiz para pegar o último atributo
                    var matriculaMestre = _context.MatriculaMestre
                        .Where(mm => mm.MestreId == material.MestreId && mm.AprendizId == aprendizId)
                        .OrderByDescending(mm => mm.Id)  // Supondo que o último atributo seja o de maior ID
                        .FirstOrDefault();

                    if (matriculaMestre != null)
                    {
                        // Criar uma nova entrada em MaterialMatriculaMestre
                        var materialMatriculaMestre = new MaterialMatriculaMestre
                        {
                            MaterialId = material.Id,
                            MatriculaMestreId = matriculaMestre.Id,
                            DataEnviado = DateTime.Now
                        };

                        _context.MaterialMatriculaMestre.Add(materialMatriculaMestre);
                    }
                }

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Policy = "Aprendiz")]
        public IActionResult MateriaisEnviados() {
            // Obter o ID do aprendiz da sessão (substitua isso pela lógica real)
            var aprendizId = Convert.ToInt32(User.Identity.Name);

            // Obter as matrículas do aprendiz
            var matriculas = _context.MatriculaMestre
            .Where(mm => mm.AprendizId == aprendizId)
            .SelectMany(mm => mm.MaterialMatriculaMestre)
            .Include(mmm => mmm.Material)
                .ThenInclude(m => m.Mestre)
            .OrderByDescending(mmm => mmm.DataEnviado).ToList();


            // Criar um modelo para a exibição na view
            var viewModel = new MateriaisEnviadosViewModel
            {
                Matriculas = matriculas
            };

            return View(viewModel);
        }
        [Authorize]
        public IActionResult Download(int materialId) {
            var material = _context.Material.Find(materialId);

            if (material == null)
            {
                return NotFound();
            }

            // Realize a lógica necessária para obter o caminho do arquivo
            var filePath = material.CaminhoArquivo;

            // Obtenha o conteúdo do arquivo como bytes
            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            // Use FileExtensionContentTypeProvider para obter o tipo MIME com base na extensão do arquivo
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            if (!contentTypeProvider.TryGetContentType(filePath, out var contentType))
            {
                // Se não for possível determinar o tipo MIME, use o padrão "application/octet-stream"
                contentType = "application/octet-stream";
            }

            // Retorne o arquivo para download com a extensão original e o nome correto do arquivo
            return File(fileBytes, contentType, material.NomeArquivo);
        }




    }
}
