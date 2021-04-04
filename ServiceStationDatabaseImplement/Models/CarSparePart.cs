namespace ServiceStationDatabaseImplement.Models
{
    public class CarSparePart
    {
        public int Id { get; set; }
        public int SparePartId { get; set; }
        public int CarId { get; set; }
        public virtual SparePart SparePart { get; set; }
        public virtual Car Car { get; set; }
    }
}
