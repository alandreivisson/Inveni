using System.ComponentModel.DataAnnotations;

namespace Inveni.Models {
    public class MaterialMatriculaMestre {
            public int Id { get; set; }

            [Required]
            public int MaterialId { get; set; }

            [Required]
            public int MatriculaMestreId { get; set; }

            [Required]
            public DateTime DataEnviado { get; set; }

            // Propriedades de navegação
            public virtual Material? Material { get; set; }
            public virtual MatriculaMestre? MatriculaMestre { get; set; }
        
    }
}
