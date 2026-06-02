using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Void.API.Models
{
    [Table("ALERTA_CRITICO")]
    public class AlertaCriticoEntity
    {
        [Key]
        [Column("ID_ALERTA")]
        public int Id { get; set; }

        [Column("PACIENTE_ID")]
        public int PacienteId { get; set; }

        [Column("DATA_SESSAO")]
        public DateTime DataSessao { get; set; }

        [Column("TIMESTAMP_ALERTA")]
        public DateTime TimestampAlerta { get; set; }

        [Column("NIVEL_ATINGIDO", TypeName = "decimal(5,2)")]
        public decimal? NivelAtingido { get; set; }
    }
}