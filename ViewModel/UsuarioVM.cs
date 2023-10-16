using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Inveni.ViewModel
{
    public class UsuarioVM
    {
        [MaxLength(200)]
        [EmailAddress]
        [Required (ErrorMessage= "O campo Email é obrigatório!")]
        public required string Email { get; set; }
        [PasswordPropertyText]
        [Required(ErrorMessage = "O campo Senha é obrigatório!")]
        public required string Senha { get; set; }
        [Required(ErrorMessage = "Selecione uma opção das opções!")]
        public required string Opcoes { get; set; }
    }
}
