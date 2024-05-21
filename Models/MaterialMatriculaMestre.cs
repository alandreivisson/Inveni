using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inveni.Models {
    public class MaterialMatriculaMestre {
            public int Id { get; set; }

            [Required]
            public int MaterialId { get; set; }

            [Required]
            public int MatriculaMestreId { get; set; }

            [Required]
            public DateTime DataEnviado { get; set; }
        
            [ForeignKey("MaterialId")]
            public virtual Material? Material { get; set; }
            [ForeignKey("MatriculaMestreId")]
            [InverseProperty("MaterialMatriculaMestre")]
            public virtual MatriculaMestre? MatriculaMestre { get; set; }
        
    }
}
