using System.ComponentModel.DataAnnotations;

namespace Inveni.Models
{
    public class UsuarioPerfil
    {
        [Key]
        public int Id { get; set; }
        public required int UsuarioId { get; set; }
        public required int PerfilId { get; set; }
        public virtual Usuario? Usuario { get; set; }
        public virtual Perfil? Perfil { get; set; }
    }
}
