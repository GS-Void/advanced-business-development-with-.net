using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Void.API.Models
{
    [Table("SENSOR_WEARABLE")]
    public class SensorWearableEntity
    {
        [Key]
        [Column("ID_SENSOR")]
        public int Id { get; set; }

        [Required]
        [StringLength(17)]
        [Column("MAC_ADDRESS")]
        public string MacAddress { get; set; }

        [StringLength(10)]
        [Column("STATUS")]
        public string Status { get; set; } = "ATIVO";
    }
}