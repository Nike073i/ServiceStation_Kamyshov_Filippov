using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceStationDatabaseImplement.Models
{
    public class WorkDuration
    {
        [Key]
        [ForeignKey("Work")]
        public int WorkId { get; set; }

        [Required]
        public int Duration { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual Work Work { get; set; }
    }
}
