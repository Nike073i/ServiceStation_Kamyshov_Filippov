using System;
using System.ComponentModel;

namespace ServiceStationBusinessLogic.ViewModels
{
    public class ServiceRecordingViewModel
    {
        public int Id { get; set; }
        public DateTime DatePassed { get; set; }
        [DisplayName("Дата прохождения")]
        public string DatePassedString { get; set; }
        public int UserId { get; set; }

        [DisplayName("ФИО работника")]
        public string UserFIO { get; set; }
        public int CarId { get; set; }

        [DisplayName("Машина")]
        public string CarName { get; set; }
        public int TechnicalMaintenanceId { get; set; }

        [DisplayName("TO")]
        public string TechnicalMaintenanceName { get; set; }
    }
}
