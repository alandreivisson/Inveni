using System.ComponentModel.DataAnnotations;

namespace Inveni.Models
{
    public class Modelo
    {
        [Key]
        public int Id { get; set; }
        [Display (Name ="Modelo")]
        public required string Descricao { get; set; }

        public virtual ICollection<TematicaMestre>? TematicaMestre { get; set; }
    }
}
