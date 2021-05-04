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
    public class WorkDurationStorage : IWorkDurationStorage
    {
        public WorkDuration CreateModel(WorkDurationBindingModel model, WorkDuration workDuration)
        {
            workDuration.WorkId = model.WorkId;
            workDuration.Duration = model.Duration;
            workDuration.UserId = (int)model.UserId;
            return workDuration;
        }

        public WorkDurationViewModel GetElement(WorkDurationBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                var workDuration = context.WorkDurations
                    .Include(rec => rec.User)
                    .Include(rec => rec.Work)
                    .FirstOrDefault(rec => rec.WorkId == model.WorkId);
                return workDuration != null ?
                new WorkDurationViewModel
                {
                    WorkId = workDuration.Work.Id,
                    WorkName = workDuration.Work.WorkName,
                    Duration = workDuration.Duration,
                    UserId = workDuration.UserId,
                    UserFIO = workDuration.User.FIO
                } : null;
            }
        }

        public List<WorkDurationViewModel> GetFilteredList(WorkDurationBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new ServiceStationDatabase())
            {
                return context.WorkDurations
                    .Include(rec => rec.User)
                    .Include(rec => rec.Work)
                    .Where(rec => (!model.TimeFrom.HasValue && !model.TimeTo.HasValue && rec.Duration == model.Duration)
                    || (model.TimeFrom.HasValue && model.TimeTo.HasValue && rec.Duration >= model.TimeFrom.Value && rec.Duration <= model.TimeTo.Value)
                    || (model.UserId.HasValue && rec.UserId == model.UserId))
                    .Select(rec => new WorkDurationViewModel
                    {
                        WorkId = rec.Work.Id,
                        WorkName = rec.Work.WorkName,
                        Duration = rec.Duration,
                        UserId = rec.UserId,
                        UserFIO = rec.User.FIO
                    }).ToList();
            }
        }

        public List<WorkDurationViewModel> GetFullList()
        {
            using (var context = new ServiceStationDatabase())
            {
                return context.WorkDurations
                    .Include(rec => rec.User)
                    .Include(rec => rec.Work)
                    .Select(rec => new WorkDurationViewModel
                    {
                        WorkId = rec.Work.Id,
                        WorkName = rec.Work.WorkName,
                        Duration = rec.Duration,
                        UserId = rec.UserId,
                        UserFIO = rec.User.FIO
                    }).ToList();
            }
        }

        public void Insert(WorkDurationBindingModel model)
        {
            using (var context = new ServiceStationDatabase())
            {
                context.WorkDurations.Add(CreateModel(model, new WorkDuration()));
                context.SaveChanges();
            }
        }

        public void Update(WorkDurationBindingModel model)
        {
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                var workDuration = context.WorkDurations.FirstOrDefault(rec => rec.WorkId == model.WorkId);
                if (workDuration == null)
                {
                    throw new Exception("Продолжительность не найдена");
                }
                CreateModel(model, workDuration);
                context.SaveChanges();
            }
        }

        public void Delete(WorkDurationBindingModel model)
        {
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                var workDuration = context.WorkDurations.FirstOrDefault(rec => rec.WorkId == model.WorkId);
                if (workDuration != null)
                {
                    context.WorkDurations.Remove(workDuration);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Продолжительность не найдена");
                }
            }
        }
    }
}
