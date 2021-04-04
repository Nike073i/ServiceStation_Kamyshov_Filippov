using ServiceStationBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace ServiceStationBusinessLogic.HelperModels
{
    public class ListCarInfoStorekeeper
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public Dictionary<int, ReportCarWorkViewModel> CarWork { get; set; }
    }
}
