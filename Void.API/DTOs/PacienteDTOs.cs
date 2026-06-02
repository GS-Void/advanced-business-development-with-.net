using System.ComponentModel.DataAnnotations;

namespace Void.API.DTOs
{
    public class PacienteRequestDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11)]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(0.1, 100.0, ErrorMessage = "O limite de esforço deve ser entre 0.1 e 100.0")]
        public decimal LimiteEsforcoCritico { get; set; }
    }

    public class PacienteResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public decimal LimiteEsforcoCritico { get; set; }
    }
}