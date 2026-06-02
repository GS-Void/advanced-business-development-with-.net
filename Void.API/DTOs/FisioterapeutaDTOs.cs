using System.ComponentModel.DataAnnotations;

namespace Void.API.DTOs
{
    public class FisioterapeutaRequestDTO
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

        [Required(ErrorMessage = "O Registro Profissional (CREFITO) é obrigatório.")]
        [StringLength(20)]
        public string RegistroProfissional { get; set; }
    }

    public class FisioterapeutaResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string RegistroProfissional { get; set; }
    }
}