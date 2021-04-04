using System.Collections.Generic;
using System.ComponentModel;

namespace ServiceStationBusinessLogic.ViewModels
{
    public class TechnicalMaintenanceViewModel
    {
        public int Id { get; set; }

        [DisplayName("ТО")]
        public string TechnicalMaintenanceName { get; set; }

        [DisplayName("Сумма")]
        public decimal Sum { get; set; }
        public int UserId { get; set; }

        [DisplayName("ФИО работника")]
        public string UserFIO { get; set; }
        public Dictionary<int, string> TechnicalMaintenanceCars { get; set; }
        public Dictionary<int, (string, int)> TechnicalMaintenanceWorks { get; set; }
    }
}
