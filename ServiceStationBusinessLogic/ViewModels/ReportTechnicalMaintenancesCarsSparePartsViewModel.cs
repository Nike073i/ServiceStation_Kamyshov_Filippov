using System;

namespace ServiceStationBusinessLogic.ViewModels
{
    public class ReportTechnicalMaintenancesCarsSparePartsViewModel
    {
        public string TechnicalMaintenanceName { get; set; }
        public DateTime DatePassed { get; set; }
        public string SparePart { get; set; }
        public string CarName { get; set; }
    }
}
