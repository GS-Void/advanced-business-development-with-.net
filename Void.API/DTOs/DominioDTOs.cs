using System.ComponentModel.DataAnnotations;

namespace Void.API.DTOs
{
    // SENSOR WEARABLE 
    public class SensorRequestDTO
    {
        [Required(ErrorMessage = "O Endereço MAC é obrigatório.")]
        [StringLength(17, MinimumLength = 17)]
        public string MacAddress { get; set; }

        [StringLength(10)]
        public string Status { get; set; } = "ATIVO";
    }

    public class SensorResponseDTO
    {
        public int Id { get; set; }
        public string MacAddress { get; set; }
        public string Status { get; set; }
    }

    // PROTOCOLO ESPACIAL 
    public class ProtocoloRequestDTO
    {
        [Required]
        [StringLength(100)]
        public string NomeProtocolo { get; set; }

        [Required]
        [Range(0.1, 100.0)]
        public decimal LimiteFadigaMaxima { get; set; }
    }

    public class ProtocoloResponseDTO
    {
        public int Id { get; set; }
        public string NomeProtocolo { get; set; }
        public decimal LimiteFadigaMaxima { get; set; }
    }

    // TELEMETRIA IoT (ESP32)
    public class TelemetriaIoTRequestDTO
    {
        [Required]
        public int PacienteId { get; set; }

        [Required]
        public DateTime DataSessao { get; set; }

        [Required]
        public string DadosJson { get; set; } 
    }
}