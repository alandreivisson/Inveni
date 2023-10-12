namespace Inveni.Models
{
    public class UsuarioPerfil
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int PerfilId { get; set; }
        public required virtual Usuario? Usuario { get; set; }
        public required virtual Perfil? Perfil { get; set; }

    }
}
    