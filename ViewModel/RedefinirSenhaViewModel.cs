using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Inveni.ViewModel {
    public class RedefinirSenhaViewModel {
        [Required]
        public string Token { get; set; }

        [EmailAddress]
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O campo Email é obrigatório!")]
        public string Email { get; set; }

        [PasswordPropertyText]
        [Required(ErrorMessage = "O campo Senha é obrigatório!")]
        [RegularExpression("^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()_+{}:;|<>,/?\\]]).*$", ErrorMessage = "A senha deve conter no mínimo 8 caracteres, um símbolo especial, 1 número, 1 letra maiúscula e 1 minúscula.")]
        public string NovaSenha { get; set; }

        [PasswordPropertyText]
        [Compare(nameof(NovaSenha), ErrorMessage = "A confirmação de senha não confere com a senha. Ambas têm que possuir valores iguais.")]
        [Required(ErrorMessage = "O campo Confirmação Senha é obrigatório!")]
        public string ConfNovaSenha { get; set; }
    }
}

