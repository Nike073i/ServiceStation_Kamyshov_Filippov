using ServiceStationBusinessLogic.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceStationDatabaseImplement.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FIO { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public UserPosition Position { get; set; }

        [ForeignKey("UserId")]
        public virtual List<TechnicalMaintenance> TechnicalMaintenances { get; set; }

        [ForeignKey("UserId")]
        public virtual List<Car> Cars { get; set; }

        [ForeignKey("UserId")]
        public virtual List<ServiceRecording> ServiceRecordings { get; set; }

        [ForeignKey("UserId")]
        public virtual List<SparePart> SpareParts { get; set; }

        [ForeignKey("UserId")]
        public virtual List<Work> Works { get; set; }

        [ForeignKey("UserId")]
        public virtual List<WorkDuration> WorkDurations { get; set; }
    }
}
