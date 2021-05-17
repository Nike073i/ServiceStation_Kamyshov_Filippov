using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace ServiceStationBusinessLogic.HelperModels
{
    public class PdfInfoStorekeeper
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public ReportInfoesStorekeeper ReportInfoes { get; set; }
    }
}
