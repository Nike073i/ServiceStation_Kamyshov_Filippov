using System.Collections.Generic;
using ServiceStationBusinessLogic.ViewModels;

namespace ServiceStationBusinessLogic.HelperModels
{
   public class ListSparePartsInfoWorker
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<ReportTechnicalMaintenanceSparePartsViewModel> TechnicalMaintenanceSpareParts { get; set; }
    }
}
