using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.HelperModels;
using ServiceStationBusinessLogic.Interfaces;
using ServiceStationBusinessLogic.ViewModels;
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
            return record;
        }

        // Получение списка запчастей с указанием работ и машин за определенный период
        public List<ReportSparePartsViewModel> GetSparePartWorkCar(ReportStorekeeperBindingModel model)
        {
            var serviceRecordings = _serviceRecording.GetFilteredList(new ServiceRecordingBindingModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            });

            var sparePartWorkCar = new List<ReportSparePartsViewModel>();
            serviceRecordings.ForEach(serviceRecording =>
            {
                var technicalMaintenances = _technicalMaintenanceStorage.GetElement(new TechnicalMaintenanceBindingModel
                {
                    Id = serviceRecording.TechnicalMaintenanceId
                });
                technicalMaintenances.TechnicalMaintenanceWorks.ToList().ForEach(technicalMaintenance =>
                {
                    var work = _workStorage.GetElement(new WorkBindingModel
                    {
                        Id = technicalMaintenance.Key
                    });
                    work.WorkSpareParts.ToList().ForEach(sparePart =>
                    {
                        sparePartWorkCar.Add(new ReportSparePartsViewModel
                        {
                            CarName = serviceRecording.CarName,
                            DatePassed = serviceRecording.DatePassed,
                            WorkName = work.WorkName,
                            SparePart = sparePart.Value.Item1
                        });
                    });
                });
            });
            return sparePartWorkCar;
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
                SparePartWorkCar = GetSparePartWorkCar(model)
            });
        }
    }
}
