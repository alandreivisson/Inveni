using System.ComponentModel.DataAnnotations;

namespace Inveni.Models {
    public class Favoritado {
        [Key]
        public int Id { get; set; }
        [Required]
        public int AprendizId { get; set; }
        [Required]
        public int TematicasMestreId { get; set; }

        public virtual Usuario? Aprendiz { get; set; }
        public virtual TematicaMestre? TematicaMestre { get; set; }
    }
}
