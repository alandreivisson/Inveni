using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inveni.Models {
    public class Notificacao {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Descricao { get; set; }

        [Required]
        public bool Aberto { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
    }
}
