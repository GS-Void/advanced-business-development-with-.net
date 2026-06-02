using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Void.API.Models
{
    [Table("TB_VOID_FISIOTERAPEUTA")]
    public class FisioterapeutaEntity : UsuarioEntity
    {
        [Required]
        [StringLength(20)]
        [Column("REGISTRO_PROFISSIONAL")]
        public string RegistroProfissional { get; set; }
    }
}