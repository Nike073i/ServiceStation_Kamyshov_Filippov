using System.Collections.Generic;
using System.ComponentModel;

namespace ServiceStationBusinessLogic.ViewModels
{
    public class WorkViewModel
    {
        public int Id { get; set; }

        [DisplayName("Работа")]
        public string WorkName { get; set; }

        [DisplayName("Цена")]
        public decimal Price { get; set; }
        public int UserId { get; set; }

        [DisplayName("ФИО кладовщика")]
        public string UserFIO { get; set; }
        public Dictionary<int, (string, int)> WorkSpareParts { get; set; }
    }
}
