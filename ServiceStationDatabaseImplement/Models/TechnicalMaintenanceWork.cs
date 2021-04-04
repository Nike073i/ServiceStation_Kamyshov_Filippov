using System.ComponentModel.DataAnnotations;

namespace ServiceStationDatabaseImplement.Models
{
    public class TechnicalMaintenanceWork
    {
        public int Id { get; set; }

        [Required]
        public int Count { get; set; }
        public int TechnicalMaintenanceId { get; set; }
        public int WorkId { get; set; }
        public virtual TechnicalMaintenance TechnicalMaintenance { get; set; }
        public virtual Work Work { get; set; }
    }
}
