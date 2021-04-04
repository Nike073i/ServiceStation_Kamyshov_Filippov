using System.Collections.Generic;

namespace ServiceStationBusinessLogic.ViewModels
{
    public class ReportCarWorkViewModel
    {
        public string CarName { get; set; }
        public Dictionary<int, (string, decimal)> Works { get; set; }
    }
}
