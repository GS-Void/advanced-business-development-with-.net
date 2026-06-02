using System.ComponentModel.DataAnnotations.Schema;

namespace Void.API.Models
{
    [Table("LEITURA_FADIGA")]
    public class LeituraFadigaEntity
    {
        [Column("PACIENTE_ID")]
        public int PacienteId { get; set; }

        [Column("DATA_SESSAO")]
        public DateTime DataSessao { get; set; }

        [Column("SEGUNDO_LEITURA")]
        public int SegundoLeitura { get; set; }

        [Column("ID_SENSOR")]
        public int IdSensor { get; set; }

        [Column("PERCENTUAL_DESGASTE", TypeName = "decimal(5,2)")]
        public decimal PercentualDesgaste { get; set; }

        public SessaoReabilitacaoEntity Sessao { get; set; }
        public SensorWearableEntity Sensor { get; set; }
    }
}