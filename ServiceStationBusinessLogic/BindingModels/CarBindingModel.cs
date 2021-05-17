using System.Collections.Generic;

namespace ServiceStationBusinessLogic.BindingModels
{
    public class CarBindingModel
    {
        public int? Id { get; set; }
        public string CarName { get; set; }
        public int? UserId { get; set; }
        public Dictionary<int, string> CarSpareParts { get; set; }
    }
}
