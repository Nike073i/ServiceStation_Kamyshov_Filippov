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
    public class TechnicalMaintenanceStorage : ITechnicalMaintenanceStorage
    {
        public TechnicalMaintenance CreateModel(TechnicalMaintenanceBindingModel model, TechnicalMaintenance technicalMaintenance, ServiceStationDatabase context)
        {
            technicalMaintenance.TechnicalMaintenanceName = model.TechnicalMaintenanceName;
            technicalMaintenance.Sum = model.Sum;
            technicalMaintenance.UserId = (int) model.UserId;
            if (technicalMaintenance.Id == 0)
            {
                context.TechnicalMaintenances.Add(technicalMaintenance);
                context.SaveChanges();
            }
            if (model.Id.HasValue)
            {
                List<TechnicalMaintenanceWork> technicalMaintenanceWorks = context.TechnicalMaintenanceWorks
                    .Where(rec => rec.TechnicalMaintenanceId == model.Id.Value).ToList();
                List<TechnicalMaintenanceCar> technicalMaintenanceCars = context.TechnicalMaintenanceCars
                    .Where(rec => rec.TechnicalMaintenanceId == model.Id.Value).ToList();
                // удалили те, которых нет в модели
                context.TechnicalMaintenanceWorks
                    .RemoveRange(technicalMaintenanceWorks
                    .Where(rec => !model.TechnicalMaintenanceWorks.ContainsKey(rec.WorkId)).ToList());
                context.SaveChanges();
                context.TechnicalMaintenanceCars
                    .RemoveRange(technicalMaintenanceCars
                    .Where(rec => !model.TechnicalMaintenanceCars.ContainsKey(rec.CarId)).ToList());
                context.SaveChanges();
                // обновили количество у существующих записей
                foreach (var updateWork in technicalMaintenanceWorks)
                {
                    if (model.TechnicalMaintenanceWorks.ContainsKey(updateWork.WorkId))
                    {
                        updateWork.Count = model.TechnicalMaintenanceWorks[updateWork.WorkId].Item2;
                        model.TechnicalMaintenanceWorks.Remove(updateWork.WorkId);
                    }
                }
                context.SaveChanges();
                // убираем повторы
                foreach (var techicalMaintenanceCar in technicalMaintenanceCars)
                {
                    if (model.TechnicalMaintenanceCars.ContainsKey(techicalMaintenanceCar.CarId))
                    {
                        model.TechnicalMaintenanceCars.Remove(techicalMaintenanceCar.CarId);
                    }
                }
                context.SaveChanges();
            }
            // добавили новые
            foreach (KeyValuePair<int, (string, int)> TMW in model.TechnicalMaintenanceWorks)
            {
                context.TechnicalMaintenanceWorks.Add(new TechnicalMaintenanceWork
                {
                    TechnicalMaintenanceId = technicalMaintenance.Id,
                    WorkId = TMW.Key,
                    Count = TMW.Value.Item2
                });
                context.SaveChanges();
            }
            foreach (KeyValuePair<int, string> TMC in model.TechnicalMaintenanceCars)
            {
                context.TechnicalMaintenanceCars.Add(new TechnicalMaintenanceCar
                {
                    TechnicalMaintenanceId = technicalMaintenance.Id,
                    CarId = TMC.Key,
                });
                context.SaveChanges();
            }
            return technicalMaintenance;
        }

        public TechnicalMaintenanceViewModel GetElement(TechnicalMaintenanceBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                var technicalMaintenance = context.TechnicalMaintenances
                    .Include(rec => rec.TechnicalMaintenanceWorks)
                    .ThenInclude(rec => rec.Work)
                    .Include(rec => rec.TechnicalMaintenanceCars)
                    .ThenInclude(rec => rec.Car)
                    .Include(rec => rec.User)
                    .FirstOrDefault(rec => rec.Id == model.Id || rec.TechnicalMaintenanceName == model.TechnicalMaintenanceName);
                return technicalMaintenance != null ?
                new TechnicalMaintenanceViewModel
                {
                    Id = technicalMaintenance.Id,
                    TechnicalMaintenanceName = technicalMaintenance.TechnicalMaintenanceName,
                    Sum = technicalMaintenance.Sum,
                    UserId = technicalMaintenance.UserId,
                    UserFIO = technicalMaintenance.User.FIO,
                    TechnicalMaintenanceCars = technicalMaintenance.TechnicalMaintenanceCars.ToDictionary(recTMC => recTMC.CarId, recWSP => recWSP.Car?.CarName),
                    TechnicalMaintenanceWorks = technicalMaintenance.TechnicalMaintenanceWorks.ToDictionary(recTMW => recTMW.WorkId, recTMW => (recTMW.Work?.WorkName, recTMW.Count))
                } :
                null;
            }
        }

        public List<TechnicalMaintenanceViewModel> GetFilteredList(TechnicalMaintenanceBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new ServiceStationDatabase())
            {
                return context.TechnicalMaintenances.Include(rec => rec.TechnicalMaintenanceWorks)
                    .ThenInclude(rec => rec.Work)
                    .Include(rec => rec.TechnicalMaintenanceCars)
                    .ThenInclude(rec => rec.Car)
                    .Include(rec => rec.User)
                    .Where(rec => rec.TechnicalMaintenanceName.Contains(model.TechnicalMaintenanceName)
                    || (model.UserId.HasValue && rec.UserId == model.UserId)
                    || (model.SelectedWorks != null && rec.TechnicalMaintenanceWorks.Any(recTMW => model.SelectedWorks.Contains(recTMW.WorkId))))
                    .ToList()
                    .Select(rec => new TechnicalMaintenanceViewModel
                    {
                        Id = rec.Id,
                        TechnicalMaintenanceName = rec.TechnicalMaintenanceName,
                        Sum = rec.Sum,
                        UserId = rec.UserId,
                        UserFIO = rec.User.FIO,
                        TechnicalMaintenanceCars = rec.TechnicalMaintenanceCars.ToDictionary(recTMC => recTMC.CarId, recWSP => recWSP.Car?.CarName),
                        TechnicalMaintenanceWorks = rec.TechnicalMaintenanceWorks.ToDictionary(recTMW => recTMW.WorkId, recTMW => (recTMW.Work?.WorkName, recTMW.Count))
                    }).ToList();
            }
        }

        public List<TechnicalMaintenanceViewModel> GetFullList()
        {
            using (var context = new ServiceStationDatabase())
            {
                return context.TechnicalMaintenances.Include(rec => rec.TechnicalMaintenanceWorks)
                    .ThenInclude(rec => rec.Work)
                    .Include(rec => rec.TechnicalMaintenanceCars)
                    .ThenInclude(rec => rec.Car)
                    .Include(rec => rec.User)
                    .ToList()
                    .Select(rec => new TechnicalMaintenanceViewModel
                    {
                        Id = rec.Id,
                        TechnicalMaintenanceName = rec.TechnicalMaintenanceName,
                        Sum = rec.Sum,
                        UserId = rec.UserId,
                        UserFIO = rec.User.FIO,
                        TechnicalMaintenanceCars = rec.TechnicalMaintenanceCars.ToDictionary(recTMC => recTMC.CarId, recWSP => recWSP.Car?.CarName),
                        TechnicalMaintenanceWorks = rec.TechnicalMaintenanceWorks.ToDictionary(recTMW => recTMW.WorkId, recTMW => (recTMW.Work?.WorkName, recTMW.Count))
                    }).ToList();
            }
        }

        public void Insert(TechnicalMaintenanceBindingModel model)
        {
            using (var context = new ServiceStationDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new TechnicalMaintenance(), context);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(TechnicalMaintenanceBindingModel model)
        {
            using (var context = new ServiceStationDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var technicalMaintenance = context.TechnicalMaintenances
                            .FirstOrDefault(rec => rec.Id == model.Id || rec.TechnicalMaintenanceName == model.TechnicalMaintenanceName);
                        if (technicalMaintenance == null)
                        {
                            throw new Exception("ТО не найдено");
                        }
                        CreateModel(model, technicalMaintenance, context);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Delete(TechnicalMaintenanceBindingModel model)
        {
            using (var context = new ServiceStationDatabase())
            {
                var technicalMaintenance = context.TechnicalMaintenances
                    .FirstOrDefault(rec => rec.Id == model.Id || rec.TechnicalMaintenanceName == model.TechnicalMaintenanceName);
                if (technicalMaintenance != null)
                {
                    context.TechnicalMaintenances.Remove(technicalMaintenance);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("ТО не найдено");
                }
            }
        }
    }
}
