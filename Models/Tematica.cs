using System.ComponentModel.DataAnnotations;

namespace Inveni.Models
{
    public class Tematica
    {
        [Key]
        public int Id { get; set; }
        [Display (Name = "Descrição")]
        public required string Descricao { get; set; }
        [Display (Name ="Categoria")]
        public required int CategoriaId { get; set; }
        public required virtual Categoria? Categoria { get; set;}

        public virtual ICollection<TematicaMestre>? TematicaMestre { get; set; }
    }
}
