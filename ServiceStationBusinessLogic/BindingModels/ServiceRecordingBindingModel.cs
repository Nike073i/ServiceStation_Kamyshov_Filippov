using System;

namespace ServiceStationBusinessLogic.BindingModels
{
    public class ServiceRecordingBindingModel
    {
        public int? Id { get; set; }
        public DateTime DatePassed { get; set; }
        public int? UserId { get; set; }
        public int CarId { get; set; }
        public int TechnicalMaintenanceId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool? ReportStorekeeper { get; set; }
        public bool? ReportWorker { get; set; }
    }
}
