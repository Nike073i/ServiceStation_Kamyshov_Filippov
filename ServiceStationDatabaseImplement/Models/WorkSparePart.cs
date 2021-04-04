using System.ComponentModel.DataAnnotations;

namespace ServiceStationDatabaseImplement.Models
{
    public class WorkSparePart
    {
        public int Id { get; set; }

        [Required]
        public int Count { get; set; }
        public int SparePartId { get; set; }
        public int WorkId { get; set; }
        public virtual SparePart SparePart { get; set; }
        public virtual Work Work { get; set; }
    }
}
