﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inveni.Models {
    public class Material {
        [Key]
        public int Id { get; set; }
        [Display (Name = "Arquivo")]
        public required string NomeArquivo { get; set; }
        public required string CaminhoArquivo { get; set; }
        public required int MestreId { get; set; }
        public virtual Usuario? Mestre { get; set; }
        public virtual ICollection<MaterialMatriculaMestre>? MaterialMatriculaMestre { get; set; }
        public virtual ICollection<MaterialMatricula>? MaterialMatricula { get; set; }

        [NotMapped]
        public IFormFile? Arquivo { get; set; }
    }
}
