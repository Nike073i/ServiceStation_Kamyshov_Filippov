using ServiceStationBusinessLogic.Enums;

namespace ServiceStationBusinessLogic.BindingModels
{
    public class ChangeUserPositionBindingModel
    {
        public int UserId { get; set; }
        public UserPosition Position { get; set; }
    }
}
