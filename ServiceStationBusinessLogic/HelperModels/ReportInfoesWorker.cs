using System;
using System.Collections.Generic;
using ServiceStationBusinessLogic.ViewModels;
using System.Text;

namespace ServiceStationBusinessLogic.HelperModels
{
    public class ReportInfoesWorker
    {
        public List<Tuple<string, int>> TotalCount { get; set; }
        public List<Tuple<string, int>> CountByMounth { get; set; }
    }
}
