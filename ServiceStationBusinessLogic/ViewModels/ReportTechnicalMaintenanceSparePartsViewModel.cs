using System.Collections.Generic;

namespace ServiceStationBusinessLogic.ViewModels
{
    public class ReportTechnicalMaintenanceSparePartsViewModel
    {
        public string TechnicalMaintenanceName { get; set; }
        public Dictionary<int, (string, int)> SpareParts { get; set; }
    }
}
