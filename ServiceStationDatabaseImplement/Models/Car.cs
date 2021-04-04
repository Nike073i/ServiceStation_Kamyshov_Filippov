using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceStationDatabaseImplement.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        public string CarName { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("CarId")]
        public virtual List<ServiceRecording> ServiceRecordings { get; set; }

        [ForeignKey("CarId")]
        public virtual List<TechnicalMaintenanceCar> TechnicalMaintenanceCars { get; set; }

        [ForeignKey("CarId")]
        public virtual List<CarSparePart> CarSpareParts { get; set; }
    }
}
