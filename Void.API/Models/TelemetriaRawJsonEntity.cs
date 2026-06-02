using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Void.API.Models
{
    [Table("TELEMETRIA_RAW_JSON")]
    public class TelemetriaRawJsonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_LOG")]
        public int Id { get; set; }

        [Column("PACIENTE_ID")]
        public int PacienteId { get; set; }

        [Column("DATA_SESSAO")]
        public DateTime DataSessao { get; set; }

        [Column("DADOS_JSON")]
        public string DadosJson { get; set; }
    }
}