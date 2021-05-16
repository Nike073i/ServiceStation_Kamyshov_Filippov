using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace ServiceStationBusinessLogic.BindingModels
{
    public class ReportWorkerBindingModel
    {
        public string FileName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public List<TechnicalMaintenanceViewModel> TechnicalMaintenances { get; set; }
        public int? UserId { get; set; }
    }
}
