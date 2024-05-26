using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inveni.Models {
    public class MaterialEnviadoHistorico {
        public int Id { get; set; }

        [Required]
        public int MaterialId { get; set; }

        [Required] 
        public int AprendizId { get; set; }
        [Required]
        public int MestreId { get; set; }

        [Required]
        public DateTime DataEnviado { get; set; }

        [ForeignKey("MaterialId")]
        public virtual Material? Material { get; set; }
        [ForeignKey("AprendizId")]
        [InverseProperty("MaterialEnviadoHistorico")]
        public virtual Usuario? Aprendiz { get; set; }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            MaterialEnviadoHistorico other = (MaterialEnviadoHistorico)obj;
            return MaterialId == other.MaterialId && AprendizId == other.AprendizId && DataEnviado == other.DataEnviado;
        }

        public override int GetHashCode() {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + MaterialId.GetHashCode();
                hash = hash * 23 + AprendizId.GetHashCode();
                hash = hash * 23 + DataEnviado.GetHashCode();
                return hash;
            }
        }
    }
}
