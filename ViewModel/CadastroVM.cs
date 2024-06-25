using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Inveni.ViewModel
{
    public class CadastroVM
    {


        public int Id { get; set; }

        [MaxLength(200)]
        [Required(ErrorMessage = "O campo Nome é obrigatório!")]
        public required string Nome { get; set; }

 /*       [MaxLength(200)]
        [Required(ErrorMessage = "O campo Localização é obrigatório!")]
        public required string Localizacao { get; set; }*/

        [MaxLength(200)]
        [EmailAddress (ErrorMessage = "O campo deve ser preenchido com um e-mail.")]
        [Required (ErrorMessage= "O campo Email é obrigatório!")]
        public required string Email { get; set; }
        [PasswordPropertyText]
        [Required(ErrorMessage = "O campo Senha é obrigatório!")]
        [RegularExpression("^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()_+{}:;|<>,/?\\]]).*$", ErrorMessage = "A senha deve conter no mínimo 8 caracteres, um símbolo especial, 1 número, 1 letra maiúscula e 1 minúscula.")]
        public required string Senha { get; set; }
   
        [Compare(nameof(Senha), ErrorMessage = "As senhas devem ser iguais")]
        public required string SenhaConfirmacao { get; set; }

        [Required(ErrorMessage = "Selecione uma opção das opções!")]
        public required string Opcoes { get; set; }

 

    }
}
