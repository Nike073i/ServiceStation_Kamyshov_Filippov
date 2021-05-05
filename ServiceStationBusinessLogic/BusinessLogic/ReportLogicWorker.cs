using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.HelperModels;
using ServiceStationBusinessLogic.Interfaces;
using ServiceStationBusinessLogic.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

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
            foreach (var tm in selectedTMS)
            {
                var sparePartsDict = new Dictionary<int, (string, int)>();
               foreach(var work in tm.TechnicalMaintenanceWorks)
               {
                    var view = workStorage.GetElement(new WorkBindingModel
                    {
                        Id = work.Key
                    });
                    if(view != null)
                    {
                        foreach(var sparePart in view.WorkSpareParts)
                        {
                            if (sparePartsDict.ContainsKey(sparePart.Key))
                            {
                                int currentCount = sparePartsDict[sparePart.Key].Item2;
                                currentCount += sparePart.Value.Item2*work.Value.Item2;
                                sparePartsDict[sparePart.Key] = (sparePart.Value.Item1, currentCount);
                            }
                            else 
                            {
                                sparePartsDict.Add(sparePart.Key,(sparePart.Value.Item1, sparePart.Value.Item2*work.Value.Item2));
                            }
                        }
                    }
               }
                record.Add(new ReportTechnicalMaintenanceSparePartsViewModel
                {
                    TechnicalMaintenanceName = tm.TechnicalMaintenanceName,
                    SpareParts = sparePartsDict
                });
            }
            return record;
        }

        public List<ReportTechnicalMaintenancesCarsSparePartsViewModel> GetSparePartTechnicalMaintenanceCar(ReportWorkerBindingModel model)
        {
            var serviceRecordings = serviceRecordingStorage.GetFilteredList(new ServiceRecordingBindingModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            });

            var tMSPCar = new List<ReportTechnicalMaintenancesCarsSparePartsViewModel>();
            foreach (var serviceRecording in serviceRecordings)
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
                foreach (var work in tm.TechnicalMaintenanceWorks)
                {
                    var view = workStorage.GetElement(new WorkBindingModel
                    {
                        Id = work.Key
                    });
                    if (view != null)
                    {
                        foreach (var sparePart in view.WorkSpareParts)
                        {
                            if (!sparePartsDict.ContainsKey(sparePart.Key))
                            {
                               sparePartsDict.Add(sparePart.Key, sparePart.Value.Item1);
                            }
                        }
                    }
                }
                foreach(var sparePart in sparePartsDict)
                {
                    foreach(var sparePartCar in car.CarSpareParts)
                    {
                        if(sparePart.Key == sparePartCar.Key)
                        {
                            tMSPCar.Add(new ReportTechnicalMaintenancesCarsSparePartsViewModel
                            {
                                TechnicalMaintenanceName = tm.TechnicalMaintenanceName,
                                DatePassed = serviceRecording.DatePassed,
                                CarName = car.CarName,
                                SparePart = sparePart.Value
                            });
                        }
                    }
                }
            }
            return tMSPCar;
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

        public void SendMail(string email, string fileName, string subject)
        {
            MailAddress from = new MailAddress("gentle.dan.test@gmail.com", "СТО Руки-Крюки");
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = subject;
            m.Attachments.Add(new Attachment(fileName));
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("gentle.dan.test@gmail.com", "594634Ol");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
    }
}
