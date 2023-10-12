using System.ComponentModel.DataAnnotations;

namespace Inveni.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Display (Name ="Descrição")]
        public required string Descricao { get; set; }

        public ICollection <Tematica>? Tematicas { get; set; }
    }
}
