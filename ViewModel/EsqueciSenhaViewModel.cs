using System.ComponentModel.DataAnnotations;

namespace Inveni.ViewModel {
    public class EsqueciSenhaViewModel {
        [Display(Name = "E-mail")]
        [Required(ErrorMessage ="O campo {0} é de preenchimento obrigatório.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
