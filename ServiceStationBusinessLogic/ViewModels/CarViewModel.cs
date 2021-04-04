using System.Collections.Generic;
using System.ComponentModel;

namespace ServiceStationBusinessLogic.ViewModels
{
    public class CarViewModel
    {
        public int Id { get; set; }
        [DisplayName("Машина")]
        public string CarName { get; set; }
        public int UserId { get; set; }
        [DisplayName("ФИО работника")]
        public string UserFIO { get; set; }
        public Dictionary<int, string> CarSpareParts { get; set; }
    }
}
