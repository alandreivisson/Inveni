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
        [EmailAddress]
        [Required (ErrorMessage= "O campo Email é obrigatório!")]
        public required string Email { get; set; }
        [PasswordPropertyText]
        [Required(ErrorMessage = "O campo Senha é obrigatório!")]
        public required string Senha { get; set; }
   
        [Compare(nameof(Senha), ErrorMessage = "As senhas devem ser iguais")]
        public required string SenhaConfirmacao { get; set; }

        [Required(ErrorMessage = "Selecione uma opção das opções!")]
        public required string Opcoes { get; set; }

 

    }
}
