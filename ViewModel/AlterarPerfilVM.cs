using System.ComponentModel.DataAnnotations;

namespace Inveni.ViewModel
{
    public class AlterarPerfilVM
    {


        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public required string Nome { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public required string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [MaxLength(11, ErrorMessage = "O tamanho máximo do campo {0} é de {1} caracteres.")]
        public required string Telefone { get; set; }

        [Display(Name = "Biografia")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public required string Biografia { get; set; }

    }
}
