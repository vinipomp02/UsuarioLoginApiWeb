using System.ComponentModel.DataAnnotations;

namespace UsuarioApiWeb.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        [Display(Name = "E-mail:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+=-]).{6,}$",
        ErrorMessage = "A senha deve ter pelo menos 6 caracteres, incluindo letras maiúsculas, minúsculas, números e caracteres especiais.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha:")]
        public string Password { get; set; }

        [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirme a Senha:")]
        [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmPassword { get; set; }
    }
}
