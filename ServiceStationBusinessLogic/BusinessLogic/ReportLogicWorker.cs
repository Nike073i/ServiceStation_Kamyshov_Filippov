using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.HelperModels;
using ServiceStationBusinessLogic.Interfaces;
using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceStationBusinessLogic.BusinessLogic
{
    public class ReportLogicWorker
    {
        private readonly IWorkStorage workStorage;
        private readonly IServiceRecordingStorage serviceRecordingStorage;
        private readonly ITechnicalMaintenanceStorage technicalMaintenanceStorage;
        private readonly ICarStorage carStorage;
        public ReportLogicWorker(IWorkStorage workStorage, IServiceRecordingStorage serviceRecordingStorage,
            ITechnicalMaintenanceStorage technicalMaintenanceStorage, ICarStorage carStorage)
        {
            this.workStorage = workStorage;
            this.serviceRecordingStorage = serviceRecordingStorage;
            this.technicalMaintenanceStorage = technicalMaintenanceStorage;
            this.carStorage = carStorage;
        }
        private List<ReportTechnicalMaintenanceSparePartsViewModel> GetTechnicalMaintenanceSpareParts(List<TechnicalMaintenanceViewModel> selectedTMS)
        { 
            var record = new List<ReportTechnicalMaintenanceSparePartsViewModel>();
            selectedTMS.ForEach(tm =>
            {
                var sparePartsDict = new Dictionary<int, (string, int)>();
                tm.TechnicalMaintenanceWorks.ToList().ForEach(work =>
                {
                    var view = workStorage.GetElement(new WorkBindingModel
                    {
                        Id = work.Key
                    });
                    if (view != null)
                    {
                        view.WorkSpareParts.ToList().ForEach(sparePart =>
                        {
                            if (sparePartsDict.ContainsKey(sparePart.Key))
                            {
                                int currentCount = sparePartsDict[sparePart.Key].Item2;
                                currentCount += sparePart.Value.Item2 * work.Value.Item2;
                                sparePartsDict[sparePart.Key] = (sparePart.Value.Item1, currentCount);
                            }
                            else
                            {
                                sparePartsDict.Add(sparePart.Key, (sparePart.Value.Item1, sparePart.Value.Item2 * work.Value.Item2));
                            }
                        });
                    }
                });
                record.Add(new ReportTechnicalMaintenanceSparePartsViewModel
                {
                    TechnicalMaintenanceName = tm.TechnicalMaintenanceName,
                    SpareParts = sparePartsDict
                });
            });
            return record;
        }

        public List<ReportTechnicalMaintenancesCarsSparePartsViewModel> GetSparePartTechnicalMaintenanceCar(ReportWorkerBindingModel model)
        {
             var serviceRecordings = serviceRecordingStorage.GetFilteredList(new ServiceRecordingBindingModel
             {
                 DateFrom = model.DateFrom,
                 DateTo = model.DateTo,
                 ReportWorker = true,
                 UserId = model.UserId
             });

             var tMSPCar = new List<ReportTechnicalMaintenancesCarsSparePartsViewModel>();
             serviceRecordings.ForEach(serviceRecording =>
             {
                 var tm = technicalMaintenanceStorage.GetElement(new TechnicalMaintenanceBindingModel
                 {
                     Id = serviceRecording.TechnicalMaintenanceId
                 });
                 var car = carStorage.GetElement(new CarBindingModel
                 {
                     Id = serviceRecording.CarId
                 });
                 var sparePartsDict = new Dictionary<int, string>();
                 tm.TechnicalMaintenanceWorks.ToList().ForEach(work =>
                 {
                     var view = workStorage.GetElement(new WorkBindingModel
                     {
                         Id = work.Key
                     });
                     if (view != null)
                     {
                         view.WorkSpareParts.ToList().ForEach(sparePart =>
                         {
                             if (!sparePartsDict.ContainsKey(sparePart.Key))
                             {
                                 sparePartsDict.Add(sparePart.Key, sparePart.Value.Item1);
                             }
                         });
                     }
                 });
                 sparePartsDict.ToList().Where(sP => car.CarSpareParts.Any(spC => spC.Key.Equals(sP.Key)))
                 .ToList().ForEach(tmsp =>
                 {
                     tMSPCar.Add(new ReportTechnicalMaintenancesCarsSparePartsViewModel
                     {
                         TechnicalMaintenanceName = tm.TechnicalMaintenanceName,
                         DatePassed = serviceRecording.DatePassed,
                         CarName = car.CarName,
                         SparePart = tmsp.Value
                     });
                 });
             });
            return tMSPCar
                .OrderBy(rec => rec.DatePassed)
                .ThenBy(rec => rec.TechnicalMaintenanceName)
                .ThenBy(rec => rec.CarName)
                .ThenBy(rec => rec.SparePart)
                .ToList();
        }

        public ReportInfoesWorker GetTechnicalMaintenance(ReportWorkerBindingModel model)
        {
            var serviceRecordings = serviceRecordingStorage.GetFilteredList(new ServiceRecordingBindingModel
            {
                UserId = model.UserId
            });
            
            var totalCount = serviceRecordings.GroupBy(rec => rec.TechnicalMaintenanceName).Select(rec => new Tuple<string, int>
            (rec.Key, rec.Count())).OrderBy(rec => rec.Item1).ToList();

            var countByDates = serviceRecordings.OrderBy(rec => rec.DatePassed).GroupBy(rec => new { rec.DatePassed.Year, rec.DatePassed.Month })
                .Select(rec => new Tuple<string, int>((string.Format("{0}/{1}", rec.Key.Month, rec.Key.Year)), rec.Count())).ToList();

           
            return new ReportInfoesWorker 
            {
                TotalCount = totalCount,
                CountByMounth = countByDates
            };
        }

        public void SaveTechnicalMaintenanceSparePartsToWordFile(ReportWorkerBindingModel model)
        {
            SaveToWordWorker.CreateDoc(new ListSparePartsInfoWorker
            {
                FileName = model.FileName,
                Title = "Список запчастей по указанным ТО",
                TechnicalMaintenanceSpareParts = GetTechnicalMaintenanceSpareParts(model.TechnicalMaintenances)
            });
        }

        public void SaveTechnicalMaintenanceSparePartsToExcelFile(ReportWorkerBindingModel model)
        {
            SaveToExcelWorker.CreateDoc(new ListSparePartsInfoWorker
            {
                FileName = model.FileName,
                Title = "Список запчастей по указанным ТО",
                TechnicalMaintenanceSpareParts= GetTechnicalMaintenanceSpareParts(model.TechnicalMaintenances)
            });
        }
        public void SaveSparePartsToPdfFile(ReportWorkerBindingModel model)
        {
            SaveToPdfWorker.CreateDoc(new PdfInfoWorker
            {
                FileName = model.FileName,
                Title = "Список пройденных ТО",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                TechnicalMaintenanceSparePartCars = GetSparePartTechnicalMaintenanceCar(model)
            });
        }
    }
}
