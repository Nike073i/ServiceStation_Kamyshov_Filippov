using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceStationDatabaseImplement.Models
{
    public class SparePart
    {
        public int Id { get; set; }

        [Required]
        public string SparePartName { get; set; }

        [Required]
        public decimal SparePartPrice { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("SparePartId")]
        public virtual List<CarSparePart> CarSpareParts { get; set; }

        [ForeignKey("SparePartId")]
        public virtual List<WorkSparePart> SparePartWorks { get; set; }
    }
}
