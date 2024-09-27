using System.ComponentModel.DataAnnotations;

namespace UsuarioApiWeb.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O Email não é válido.")]
        [Display(Name = "E-mail:")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha:")]
        public string Password { get; set; }
    }
}
