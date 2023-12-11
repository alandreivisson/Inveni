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
        public string? Biografia { get; set; }

        [MaxLength(200)]
        public string? Telefone { get; set; }

        [PasswordPropertyText]
        public required string Senha { get; set; }
        [Display (Name ="Ativo")]
        public required bool Ativo { get; set; }

        [MaxLength(500)]
        public string? CaminhoFoto { get; set; }

        public ICollection<UsuarioPerfil>? UsuarioPerfil;

        public virtual ICollection<TematicaMestre>? TematicaMestre { get; set; }

        public virtual ICollection<Favorito>? Favoritos { get; set; }

        public virtual ICollection<Matricula>? Matriculas { get; set; }

        public virtual ICollection<MatriculaMestre>? MatriculaMestreMestre { get; set; }

        public virtual ICollection<MatriculaMestre>? MatriculaMestreAprendiz { get; set; }
    }
}
