using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Void.API.Models
{
    [Table("LOG_AUDITORIA_SESSAO")]
    [Keyless] 
    public class LogAuditoriaSessaoEntity
    {
        [Column("DATA_HORA")]
        public DateTime? DataHora { get; set; }

        [StringLength(20)]
        [Column("ACAO")]
        public string Acao { get; set; }

        [Column("PACIENTE_ID")]
        public int? PacienteId { get; set; }

        [Column("DATA_SESSAO")]
        public DateTime? DataSessao { get; set; }

        [StringLength(20)]
        [Column("STATUS_ANTIGO")]
        public string StatusAntigo { get; set; }
    }
}