using System.ComponentModel.DataAnnotations.Schema;

namespace Inveni.Models {
    public class Matricula {
        public int Id { get; set; }
        public int AprendizId { get; set; }
        public int TematicaMestreId { get; set; }
        public MatriculaStatus Status { get; set; }

        [ForeignKey("AprendizId")]
        [InverseProperty("Matriculas")]
        public virtual Usuario? Aprendiz { get; set; }

        [ForeignKey("TematicaMestreId")]
        public virtual TematicaMestre? TematicaMestre { get; set; }

        public virtual ICollection<MaterialMatricula> MaterialMatricula { get; set; }
    }
    public enum MatriculaStatus {
        Matriculado,
        Pendente,
        Finalizado,
    }
}
