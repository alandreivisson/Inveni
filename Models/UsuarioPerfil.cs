using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inveni.Models
{
    public class UsuarioPerfil
    {
        [Key]
        public int Id { get; set; }
        public required int UsuarioId { get; set; }
        public required int PerfilId { get; set; }
        [ForeignKey("UsuarioId")]
        [InverseProperty("UsuarioPerfil")]
        public virtual Usuario? Usuario { get; set; }
        [ForeignKey("PerfilId")]
        public virtual Perfil? Perfil { get; set; }
    }
}
