using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Inveni.Models
{
    public class TematicaMestre
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(500)]
        [Display (Name ="Descrição")]
        public required string Biografia { get; set; }
        [Display (Name = "Temática")]
        public required int TematicaId { get; set; }
        public required int UsuarioId { get; set; }
        [Display (Name = "Modelo")]
        public required int ModeloId { get; set; }
        [Required(ErrorMessage = "Selecione uma opção das opções entre ativo ou inativo!")]
        public required bool Ativo { get; set; }
        public required virtual Tematica? Tematica { get; set; }
        public required virtual Usuario? Usuario { get; set; }
        public required virtual Modelo? Modelo { get; set; }
    }
}
