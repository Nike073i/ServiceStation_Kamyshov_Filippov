using System.Collections.Generic;

namespace ServiceStationBusinessLogic.BindingModels
{
    public class WorkBindingModel
    {
        public int? Id { get; set; }
        public string WorkName { get; set; }
        public decimal Price { get; set; }
        public int? UserId { get; set; }
        public Dictionary<int, (string, int)> WorkSpareParts { get; set; }
    }
}
