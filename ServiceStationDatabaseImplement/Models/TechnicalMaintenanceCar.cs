namespace ServiceStationDatabaseImplement.Models
{
    public class TechnicalMaintenanceCar
    {
        public int Id { get; set; }
        public int TechnicalMaintenanceId { get; set; }
        public int CarId { get; set; }
        public virtual TechnicalMaintenance TechnicalMaintenance { get; set; }
        public virtual Car Car { get; set; }
    }
}
