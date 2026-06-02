using System.ComponentModel.DataAnnotations;

namespace Void.API.DTOs
{
    public class SessaoRequestDTO
    {
        [Required]
        public int PacienteId { get; set; }

        [Required]
        public DateTime DataSessao { get; set; }

        [Required]
        public int IdFisio { get; set; }

        [Required]
        public int IdProtocolo { get; set; }

        [StringLength(20)]
        public string StatusSessao { get; set; } = "AGENDADA";
    }

    public class SessaoResponseDTO
    {
        public int PacienteId { get; set; }
        public DateTime DataSessao { get; set; }
        public decimal? DesgasteAcumulado { get; set; }
        public int? AlertaFadigaCritica { get; set; }
        public int IdFisio { get; set; }
        public int IdProtocolo { get; set; }
        public string StatusSessao { get; set; }
    }
}