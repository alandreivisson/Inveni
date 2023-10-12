using System.ComponentModel.DataAnnotations;

namespace Inveni.Models
{
    public class Perfil
    {
        public int Id { get; set; }
        
        [MaxLength(255)]
        [Display(Name = "Descrição")]
        public required string Descricao { get; set; }
        public virtual ICollection<UsuarioPerfil>? UsuarioPerfil { get; set; }

    }
}
