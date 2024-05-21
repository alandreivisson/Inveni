using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inveni.Models {
    public class MaterialMatricula {
        [Key]
        public int Id { get; set; }
        public int MaterialId { get; set; }
        [ForeignKey("MaterialId")]
        public Material Material { get; set; }
        
        public int MatriculaId { get; set; }
        [ForeignKey("MatriculaId")]
        [InverseProperty("MaterialMatricula")]
        public Matricula Matricula { get; set; }
    }
}
