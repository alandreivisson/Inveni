using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Inveni.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(200)]
        public required string Nome { get; set; }
        
        [MaxLength(200)]
        [EmailAddress]
        public required string Email { get; set; }

        [MaxLength(200)]
        public required string Biografia { get; set; }

        [MaxLength(200)]
        public required string Telefone { get; set; }

        [PasswordPropertyText]
        public required string Senha { get; set; }
        [Display (Name ="Ativo")]
        public required bool Ativo { get; set; }

        public ICollection<UsuarioPerfil>? UsuarioPerfil;

        public virtual ICollection<TematicaMestre>? TematicaMestre { get; set; }
    }
}
