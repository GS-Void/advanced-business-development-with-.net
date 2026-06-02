using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Void.API.Models
{
    [Table("TB_VOID_USUARIO")]
    public abstract class UsuarioEntity
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("NOME")]
        public string Nome { get; set; }

        [Required]
        [StringLength(11)]
        [Column("CPF")]
        public string Cpf { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        [Column("EMAIL")]
        public string Email { get; set; }

        [Required]
        [StringLength(1)]
        [Column("TIPO_USUARIO")]
        public string TipoUsuario { get; set; }
    }
}