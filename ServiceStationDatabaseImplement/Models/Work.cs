using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceStationDatabaseImplement.Models
{
    public class Work
    {
        public int Id { get; set; }

        [Required]
        public string WorkName { get; set; }

        [Required]
        public decimal WorkPrice { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual WorkDuration WorkDuration { get; set; }

        [ForeignKey("WorkId")]
        public virtual List<TechnicalMaintenanceWork> TechnicalMaintenanceWorks { get; set; }

        [ForeignKey("WorkId")]
        public virtual List<WorkSparePart> WorkSpareParts { get; set; }
    }
}
