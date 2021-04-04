using ServiceStationBusinessLogic.Enums;

namespace ServiceStationBusinessLogic.BindingModels
{
    public class UserBindingModel
    {
        public int? Id { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserPosition Position { get; set; }
    }
}
