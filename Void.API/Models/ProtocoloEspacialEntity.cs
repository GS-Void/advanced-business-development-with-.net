using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Void.API.Models
{
    [Table("PROTOCOLO_ESPACIAL")]
    public class ProtocoloEspacialEntity
    {
        [Key]
        [Column("ID_PROTOCOLO")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("NOME_PROTOCOLO")]
        public string NomeProtocolo { get; set; }

        [Column("LIMITE_FADIGA_MAXIMA", TypeName = "decimal(5,2)")]
        public decimal LimiteFadigaMaxima { get; set; }
    }
}