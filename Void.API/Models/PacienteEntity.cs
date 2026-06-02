using System.ComponentModel.DataAnnotations.Schema;

namespace Void.API.Models
{
    [Table("TB_VOID_PACIENTE")]
    public class PacienteEntity : UsuarioEntity
    {
        [Column("LIMITE_ESFORCO_CRITICO", TypeName = "decimal(5,2)")]
        public decimal LimiteEsforcoCritico { get; set; }

        // Navegação
        public ICollection<SessaoReabilitacaoEntity> Sessoes { get; set; }
    }
}