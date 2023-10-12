using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Inveni.ViewModel
{
    public class UsuarioVM
    {
        [MaxLength(200)]
        [EmailAddress]
        public required string Email { get; set; }
        [PasswordPropertyText]
        public required string Senha { get; set; }
    }
}
