using System.ComponentModel;

namespace ServiceStationBusinessLogic.ViewModels
{
    public class SparePartViewModel
    {
        public int Id { get; set; }

        [DisplayName("Запчасть")]
        public string SparePartName { get; set; }

        [DisplayName("Цена")]
        public decimal Price { get; set; }
        public int UserId { get; set; }

        [DisplayName("ФИО кладовщика")]
        public string UserFIO { get; set; }
    }
}
