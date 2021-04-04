using ServiceStationBusinessLogic.Enums;
using System.ComponentModel;

namespace ServiceStationBusinessLogic.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [DisplayName("ФИО пользователя")]
        public string FIO { get; set; }

        [DisplayName("Почта")]
        public string Email { get; set; }

        [DisplayName("Пароль")]
        public string Password { get; set; }

        [DisplayName("Должность")]
        public UserPosition Position { get; set; }
    }
}
