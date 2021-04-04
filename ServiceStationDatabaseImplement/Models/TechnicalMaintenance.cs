using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceStationDatabaseImplement.Models
{
    public class TechnicalMaintenance
    {
        public int Id { get; set; }

        [Required]
        public string TechnicalMaintenanceName { get; set; }

        [Required]
        public decimal Sum { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("TechnicalMaintenanceId")]
        public virtual List<TechnicalMaintenanceCar> TechnicalMaintenanceCars { get; set; }

        [ForeignKey("TechnicalMaintenanceId")]
        public virtual List<TechnicalMaintenanceWork> TechnicalMaintenanceWorks { get; set; }

        [ForeignKey("TechnicalMaintenanceId")]
        public virtual List<ServiceRecording> ServiceRecordings { get; set; }
    }
}
