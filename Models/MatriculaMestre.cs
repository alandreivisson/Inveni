using System.ComponentModel.DataAnnotations;

namespace Inveni.Models {
    public class MatriculaMestre {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MestreId { get; set; }

        [Required]
        public int AprendizId { get; set; }

        public MatriculaStatus Status { get; set; }

        // Propriedades de navegação
        public virtual Usuario? Mestre { get; set; }
        public virtual Usuario? Aprendiz { get; set; }

        public virtual ICollection<MaterialMatriculaMestre>? MaterialMatriculaMestre { get; set; }
    }
    public enum MatriculaMestreStatus {
        Matriculado,
        Finalizado,
    }
}
