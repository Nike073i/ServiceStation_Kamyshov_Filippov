using System;

namespace ServiceStationBusinessLogic.ViewModels
{
    public class ReportSparePartsViewModel
    {
        public string CarName { get; set; }
        public DateTime DatePassed { get; set; }
        public string SparePart { get; set; }
        public string WorkName { get; set; }
        public int Count { get; set; }
    }
}
