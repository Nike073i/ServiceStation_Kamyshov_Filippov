using System.Collections.Generic;

namespace ServiceStationBusinessLogic.BindingModels
{
    public class TechnicalMaintenanceBindingModel
    {
        public int? Id { get; set; }
        public string TechnicalMaintenanceName { get; set; }
        public decimal Sum { get; set; }
        public int? UserId { get; set; }
        public Dictionary<int, string> TechnicalMaintenanceCars { get; set; }
        public Dictionary<int, (string, int)> TechnicalMaintenanceWorks { get; set; }
        public List<int> SelectedWorks { get; set; }
    }
}
