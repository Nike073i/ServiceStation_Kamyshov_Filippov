using System.ComponentModel;

namespace ServiceStationBusinessLogic.ViewModels
{
    public class WorkDurationViewModel
    {
        public int WorkId { get; set; }

        [DisplayName("Работа")]
        public string WorkName { get; set; }

        [DisplayName("Продолжительность")]
        public int Duration { get; set; }
        public int UserId { get; set; }

        [DisplayName("ФИО кладовщика")]
        public string UserFIO { get; set; }
    }
}
