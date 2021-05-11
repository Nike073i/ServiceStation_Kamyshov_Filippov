using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.HelperModels;
using ServiceStationBusinessLogic.Interfaces;
using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceStationBusinessLogic.BusinessLogic
{
    public class ReportLogicStorekeeper
    {
        private readonly IServiceRecordingStorage _serviceRecording;
        private readonly IWorkStorage _workStorage;
        private readonly ITechnicalMaintenanceStorage _technicalMaintenanceStorage;
        public ReportLogicStorekeeper(IWorkStorage workStorage, ITechnicalMaintenanceStorage technicalMaintenanceStorage,
            IServiceRecordingStorage serviceRecording)
        {
            _workStorage = workStorage;
            _technicalMaintenanceStorage = technicalMaintenanceStorage;
            _serviceRecording = serviceRecording;
        }
        private Dictionary<int, ReportCarWorkViewModel> GetCarWork(List<WorkViewModel> selectedWorks)
        {
            var technicalMaintenances = _technicalMaintenanceStorage.GetFilteredList(new TechnicalMaintenanceBindingModel
            {
                SelectedWorks = selectedWorks.Select(rec => rec.Id)
                .ToList()
            });
            var record = new Dictionary<int, ReportCarWorkViewModel>();

            technicalMaintenances.ForEach(rec =>
            {
                var cars = rec.TechnicalMaintenanceCars.ToList();
                cars.Where(car => !record.ContainsKey(car.Key)).ToList()
                .ForEach(car =>
                {
                    record.Add(car.Key, new ReportCarWorkViewModel
                    {
                        CarName = car.Value,
                        Works = new Dictionary<int, (string, decimal)>()
                    });
                });
                var works = rec.TechnicalMaintenanceWorks
                .Where(work => selectedWorks.FirstOrDefault(selectedWork => selectedWork.Id == work.Key) != null)
                .ToList();
                cars.ForEach(car =>
                {
                    works.Where(work => !record[car.Key].Works.ContainsKey(work.Key))
                    .ToList()
                    .ForEach(work =>
                    {
                        var workModel = _workStorage.GetElement(new WorkBindingModel
                        {
                            Id = work.Key
                        });
                        record[car.Key].Works.Add(work.Key, (work.Value.Item1, workModel != null ? workModel.Price : 0));
                    });
                });
            });
            return record.OrderBy(rec => rec.Value.CarName).ToDictionary(rec => rec.Key, rec => rec.Value);
        }

        // Получение списка запчастей с указанием работ и машин за определенный период
        public ReportInfoes GetSparePartWorkCar(ReportStorekeeperBindingModel model)
        {
            var serviceRecordings = _serviceRecording.GetFilteredList(new ServiceRecordingBindingModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            });

            var listReportSpareParts = new List<ReportSparePartsViewModel>();
            serviceRecordings.ForEach(serviceRecording =>
            {
                var technicalMaintenances = _technicalMaintenanceStorage.GetElement(new TechnicalMaintenanceBindingModel
                {
                    Id = serviceRecording.TechnicalMaintenanceId
                });
                technicalMaintenances.TechnicalMaintenanceWorks.ToList().Where(rec => model.UserId.HasValue && _workStorage.GetElement(new WorkBindingModel
                { Id = rec.Key }).UserId == model.UserId.Value)
                .ToList()
                .ForEach(technicalMaintenance =>
                {
                    var work = _workStorage.GetElement(new WorkBindingModel
                    {
                        Id = technicalMaintenance.Key
                    });
                    work.WorkSpareParts.ToList().ForEach(sparePart =>
                    {
                        listReportSpareParts.Add(new ReportSparePartsViewModel
                        {
                            CarName = serviceRecording.CarName,
                            DatePassed = serviceRecording.DatePassed,
                            WorkName = work.WorkName,
                            SparePart = sparePart.Value.Item1,
                            Count = sparePart.Value.Item2
                        });
                    });
                });
            });
            var sparePartWorkCar = listReportSpareParts.OrderBy(rec => rec.DatePassed).ThenBy(rec => rec.SparePart).ToList();
            var totalCount = listReportSpareParts.GroupBy(sparePart => sparePart.SparePart).Select(rec => new Tuple<string, int>
            (rec.Key, rec.Sum(sparePart => sparePart.Count))).OrderBy(rec => rec.Item1).ToList();
            var countByDates = listReportSpareParts.OrderBy(rec => rec.DatePassed).GroupBy(rec => new { rec.DatePassed.Year, rec.DatePassed.Month })
                .Select(rec => new Tuple<string, int>((string.Format("{0}/{1}", rec.Key.Month, rec.Key.Year)), rec.Sum(sparePart => sparePart.Count))).ToList();

            return new ReportInfoes
            {
                SparePartWorkCar = sparePartWorkCar,
                TotalCount = totalCount,
                CountByDates = countByDates
            };
        }

        /// Сохранение машин с указанием работ в файл-Word
        public void SaveWorkCarsToWordFile(ReportStorekeeperBindingModel model)
        {
            SaveToWordStorekeeper.CreateDoc(new ListCarInfoStorekeeper
            {
                FileName = model.FileName,
                Title = "Список машин по указанным работам",
                CarWork = GetCarWork(model.Works)
            });
        }

        /// Сохранение машин с указанием работ в файл-Excel
        public void SaveWorkCarsToExcelFile(ReportStorekeeperBindingModel model)
        {
            SaveToExcelStorekeeper.CreateDoc(new ListCarInfoStorekeeper
            {
                FileName = model.FileName,
                Title = "Список машин по указанным работам",
                CarWork = GetCarWork(model.Works)
            });
        }

        /// Сохранение отчета продвижения запчастей в файл-Pdf
        public void SaveSparePartsToPdfFile(ReportStorekeeperBindingModel model)
        {
            SaveToPdfStorekeeper.CreateDoc(new PdfInfoStorekeeper
            {
                FileName = model.FileName,
                Title = "Список использованных запчастей",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                ReportInfoes = GetSparePartWorkCar(model)
            });
        }
    }
}
