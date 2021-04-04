using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.HelperModels;
using ServiceStationBusinessLogic.Interfaces;
using ServiceStationBusinessLogic.ViewModels;
using System.Collections.Generic;

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
            var technicalMaintenance = GetTechnicalMaintenanceByWorks(selectedWorks);
            var record = new Dictionary<int, ReportCarWorkViewModel>();
            foreach (var tm in technicalMaintenance)
            {
                foreach (var car in tm.TechnicalMaintenanceCars)
                {
                    if (!record.ContainsKey(car.Key))
                    {
                        record.Add(car.Key, new ReportCarWorkViewModel
                        {
                            CarName = car.Value,
                            Works = new Dictionary<int, (string, decimal)>()
                        });
                    }
                    foreach (var work in tm.TechnicalMaintenanceWorks)
                    {
                        foreach (var selectedWork in selectedWorks)
                        {
                            if (work.Key == selectedWork.Id)
                            {
                                if (!record[car.Key].Works.ContainsKey(work.Key))
                                {
                                    var workModel = _workStorage.GetElement(new WorkBindingModel
                                    {
                                        Id = work.Key
                                    });
                                    record[car.Key].Works.Add(work.Key, (work.Value.Item1, workModel != null ? workModel.Price : 0));
                                }
                            }
                        }
                    }
                }
            }
            return record;
        }

        private List<TechnicalMaintenanceViewModel> GetTechnicalMaintenanceByWorks(List<WorkViewModel> selectedWorks)
        {
            var technicalMaitenances = _technicalMaintenanceStorage.GetFullList();
            var neededTechnicalMaintenances = new List<TechnicalMaintenanceViewModel>();
            foreach (var tm in technicalMaitenances)
            {
                bool technicalMaitenanceIsNeeded = false;
                foreach (var work in tm.TechnicalMaintenanceWorks)
                {
                    if (technicalMaitenanceIsNeeded)
                    {
                        break;
                    }
                    foreach (var selectedWork in selectedWorks)
                    {
                        if (work.Key == selectedWork.Id)
                        {
                            technicalMaitenanceIsNeeded = true;
                            break;
                        }
                    }
                }
                if (technicalMaitenanceIsNeeded)
                {
                    if (!neededTechnicalMaintenances.Contains(tm))
                    {
                        neededTechnicalMaintenances.Add(tm);
                    }
                }
            }
            return neededTechnicalMaintenances;
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
            foreach (var serviceRecording in serviceRecordings)
            {
                var tm = _technicalMaintenanceStorage.GetElement(new TechnicalMaintenanceBindingModel
                {
                    Id = serviceRecording.TechnicalMaintenanceId
                });
                foreach (var technicalMaintenanceWork in tm.TechnicalMaintenanceWorks)
                {
                    var work = _workStorage.GetElement(new WorkBindingModel
                    {
                        Id = technicalMaintenanceWork.Key
                    });
                    foreach (var sparePart in work.WorkSpareParts)
                    {
                        sparePartWorkCar.Add(new ReportSparePartsViewModel
                        {
                            CarName = serviceRecording.CarName,
                            DatePassed = serviceRecording.DatePassed,
                            WorkName = work.WorkName,
                            SparePart = sparePart.Value.Item1
                        });
                    }
                }
            }
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
