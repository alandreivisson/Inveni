using System.ComponentModel.DataAnnotations;

namespace Inveni.Models {
    public class MaterialMatricula {
        [Key]
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }

        public int MatriculaId { get; set; }
        public Matricula Matricula { get; set; }
    }
}
