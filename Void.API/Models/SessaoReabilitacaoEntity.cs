using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Void.API.Models
{
    [Table("TB_VOID_SESSAO_REABILITACAO")]
    public class SessaoReabilitacaoEntity
    {
        [Column("PACIENTE_ID")]
        public int PacienteId { get; set; }

        [Column("DATA_SESSAO")]
        public DateTime DataSessao { get; set; }

        [Column("DESGASTE_ACUMULADO", TypeName = "decimal(5,2)")]
        public decimal? DesgasteAcumulado { get; set; }

        [Column("ALERTA_FADIGA_CRITICA")]
        public int? AlertaFadigaCritica { get; set; } = 0;

        [Column("ID_FISIO")]
        public int IdFisio { get; set; }

        [Column("ID_PROTOCOLO")]
        public int IdProtocolo { get; set; }

        [StringLength(20)]
        [Column("STATUS_SESSAO")]
        public string StatusSessao { get; set; } = "ANDAMENTO";

        // Navegações
        public PacienteEntity Paciente { get; set; }
        public FisioterapeutaEntity Fisioterapeuta { get; set; }
        public ProtocoloEspacialEntity Protocolo { get; set; }
        public ICollection<LeituraFadigaEntity> Leituras { get; set; }
    }
}