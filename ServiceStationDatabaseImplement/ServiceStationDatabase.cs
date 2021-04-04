using Microsoft.EntityFrameworkCore;
using ServiceStationDatabaseImplement.Models;

namespace ServiceStationDatabaseImplement
{
    public class ServiceStationDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ServiceStationDatabase;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<User> Users { set; get; }
        public virtual DbSet<TechnicalMaintenance> TechnicalMaintenances { set; get; }
        public virtual DbSet<Car> Cars { set; get; }
        public virtual DbSet<ServiceRecording> ServiceRecordings { set; get; }
        public virtual DbSet<CarSparePart> CarSpareParts { set; get; }
        public virtual DbSet<TechnicalMaintenanceCar> TechnicalMaintenanceCars { set; get; }
        public virtual DbSet<SparePart> SpareParts { set; get; }
        public virtual DbSet<Work> Works { set; get; }
        public virtual DbSet<WorkSparePart> WorkSpareParts { set; get; }
        public virtual DbSet<WorkDuration> WorkDurations { set; get; }
        public virtual DbSet<TechnicalMaintenanceWork> TechnicalMaintenanceWorks { set; get; }
    }
}
