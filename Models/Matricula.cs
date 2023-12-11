    namespace Inveni.Models {
        public class Matricula {
            public int Id { get; set; }
            public int AprendizId { get; set; }
            public int TematicaMestreId { get; set; }
            public MatriculaStatus Status { get; set; }

            public virtual Usuario? Aprendiz { get; set; }
            public virtual TematicaMestre? TematicaMestre { get; set; }
            public virtual ICollection<MaterialMatricula> MaterialMatricula { get; set; }

    }
        public enum MatriculaStatus {
            Matriculado,
            Pendente,
            Finalizado,
        }
    }
