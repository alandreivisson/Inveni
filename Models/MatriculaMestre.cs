using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inveni.Models {
    public class MatriculaMestre {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MestreId { get; set; }

        [Required]
        [Display(Name = "Aprendiz")]
        public int AprendizId { get; set; }

        public int? TematicaMestreId { get; set; }

        public MatriculaStatus Status { get; set; }

        // Propriedades de navegação
        public virtual Usuario? Mestre { get; set; }
        public virtual Usuario? Aprendiz { get; set; }
        public virtual TematicaMestre? TematicaMestre { get; set; }

        public virtual ICollection<MaterialMatriculaMestre>? MaterialMatriculaMestre { get; set; }
    }
    public enum MatriculaMestreStatus {
        Matriculado,
        Finalizado,
    }
}
