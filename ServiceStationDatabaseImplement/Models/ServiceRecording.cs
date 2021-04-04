using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceStationDatabaseImplement.Models
{
    public class ServiceRecording
    {
        public int Id { get; set; }

        [Required]
        public DateTime DatePassed { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public int TechnicalMaintenanceId { get; set; }
        public virtual User User { get; set; }
        public virtual Car Car { get; set; }
        public virtual TechnicalMaintenance TechnicalMaintenance { get; set; }
    }
}
