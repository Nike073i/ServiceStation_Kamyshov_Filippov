using Microsoft.EntityFrameworkCore;
using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.Interfaces;
using ServiceStationBusinessLogic.ViewModels;
using ServiceStationDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceStationDatabaseImplement.Implements
{
    public class ServiceRecordingStorage : IServiceRecordingStorage
    {
        public ServiceRecording CreateModel(ServiceRecordingBindingModel model, ServiceRecording serviceRecording)
        {
            serviceRecording.DatePassed = model.DatePassed;
            serviceRecording.CarId = model.CarId;
            serviceRecording.TechnicalMaintenanceId = model.TechnicalMaintenanceId;
            serviceRecording.UserId = (int) model.UserId;
            return serviceRecording;
        }
        public ServiceRecordingViewModel GetElement(ServiceRecordingBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                var serviceRecording = context.ServiceRecordings
                    .Include(rec => rec.User)
                    .Include(rec => rec.TechnicalMaintenance)
                    .Include(rec => rec.Car)
                    .FirstOrDefault(rec => rec.Id == model.Id);
                return serviceRecording != null ?
                new ServiceRecordingViewModel
                {
                    Id = serviceRecording.Id,
                    DatePassedString = serviceRecording.DatePassed.ToLongDateString(),
                    DatePassed = serviceRecording.DatePassed,
                    CarId = serviceRecording.CarId,
                    CarName = serviceRecording.Car.CarName,
                    TechnicalMaintenanceId = serviceRecording.TechnicalMaintenanceId,
                    TechnicalMaintenanceName = serviceRecording.TechnicalMaintenance.TechnicalMaintenanceName,
                    UserId = serviceRecording.UserId,
                    UserFIO = serviceRecording.User.FIO
                } : null;
            }
        }

        public List<ServiceRecordingViewModel> GetFilteredList(ServiceRecordingBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                return context.ServiceRecordings
                    .Include(rec => rec.User)
                    .Include(rec => rec.TechnicalMaintenance)
                    .Include(rec => rec.Car)
                    .Where(rec =>
                    (model.ReportStorekeeper.HasValue && model.ReportStorekeeper.Value && model.DateFrom <= rec.DatePassed && model.DateTo >= rec.DatePassed) 
                    ||
                    (model.ReportWorker.HasValue && model.ReportWorker.Value && model.UserId.HasValue && rec.UserId == model.UserId) 
                    ||
                    (model.UserId.HasValue && rec.UserId == model.UserId))
                    .Select(rec => new ServiceRecordingViewModel
                    {
                        Id = rec.Id,
                        DatePassedString = rec.DatePassed.ToLongDateString(),
                        DatePassed = rec.DatePassed,
                        CarId = rec.CarId,
                        CarName = rec.Car.CarName,
                        TechnicalMaintenanceId = rec.TechnicalMaintenanceId,
                        TechnicalMaintenanceName = rec.TechnicalMaintenance.TechnicalMaintenanceName,
                        UserId = rec.UserId,
                        UserFIO = rec.User.FIO
                    }).ToList();
            }
        }

        public List<ServiceRecordingViewModel> GetFullList()
        {
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                return context.ServiceRecordings
                    .Include(rec => rec.User)
                    .Include(rec => rec.TechnicalMaintenance)
                    .Include(rec => rec.Car)
                    .Select(rec => new ServiceRecordingViewModel
                    {
                        Id = rec.Id,
                        DatePassedString = rec.DatePassed.ToLongDateString(),
                        DatePassed = rec.DatePassed,
                        CarId = rec.CarId,
                        CarName = rec.Car.CarName,
                        TechnicalMaintenanceId = rec.TechnicalMaintenanceId,
                        TechnicalMaintenanceName = rec.TechnicalMaintenance.TechnicalMaintenanceName,
                        UserId = rec.UserId,
                        UserFIO = rec.User.FIO
                    }).ToList();
            }
        }

        public void Insert(ServiceRecordingBindingModel model)
        {
            using (var context = new ServiceStationDatabase())
            {
                context.ServiceRecordings.Add(CreateModel(model, new ServiceRecording()));
                context.SaveChanges();
            }
        }

        public void Update(ServiceRecordingBindingModel model)
        {
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                var serviceRecording = context.ServiceRecordings.FirstOrDefault(rec => rec.Id == model.Id);
                if (serviceRecording == null)
                {
                    throw new Exception("Запись не найдена");
                }
                CreateModel(model, serviceRecording);
                context.SaveChanges();
            }
        }
        public void Delete(ServiceRecordingBindingModel model)
        {
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                var serviceRecording = context.ServiceRecordings.FirstOrDefault(rec => rec.Id == model.Id);
                if (serviceRecording != null)
                {
                    context.ServiceRecordings.Remove(serviceRecording);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Запись не найдена");
                }
            }
        }
    }
}
