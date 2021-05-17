using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace ServiceStationBusinessLogic.HelperModels
{
    public class ReportInfoesStorekeeper
    {
        public List<ReportSparePartsViewModel> SparePartWorkCar { get; set; }
        public List<Tuple<string, int>> TotalCount { get; set; }
        public List<Tuple<string, int>> CountByDates { get; set; }
    }
}
