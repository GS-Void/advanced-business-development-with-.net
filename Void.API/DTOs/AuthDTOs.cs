using System.ComponentModel.DataAnnotations;

namespace Void.API.DTOs
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve conter exatamente 11 caracteres.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
        public string Email { get; set; }
    }

    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string Nome { get; set; }
        public string Role { get; set; } // Paciente ou Fisioterapeuta
    }
}