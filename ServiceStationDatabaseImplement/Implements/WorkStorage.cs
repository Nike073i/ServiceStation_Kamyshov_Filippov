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
    public class WorkStorage : IWorkStorage
    {
        public Work CreateModel(WorkBindingModel model, Work work, ServiceStationDatabase context)
        {
            work.WorkName = model.WorkName;
            work.WorkPrice = model.Price;
            work.UserId = (int)model.UserId;
            if (work.Id == 0)
            {
                context.Works.Add(work);
                context.SaveChanges();
            }
            if (model.Id.HasValue)
            {
                List<WorkSparePart> workSpareParts = context.WorkSpareParts.Where(rec => rec.WorkId == model.Id.Value).ToList();
                // удалили те, которых нет в модели
                context.WorkSpareParts
                    .RemoveRange(workSpareParts.Where(rec => !model.WorkSpareParts.ContainsKey(rec.SparePartId)).ToList());
                context.SaveChanges();
                // обновили количество у существующих записей
                foreach (var updateSpareParts in workSpareParts)
                {
                    if (model.WorkSpareParts.ContainsKey(updateSpareParts.SparePartId))
                    {
                        updateSpareParts.Count = model.WorkSpareParts[updateSpareParts.SparePartId].Item2;
                        model.WorkSpareParts.Remove(updateSpareParts.SparePartId);
                    }
                }
                context.SaveChanges();
            }
            // добавили новые
            foreach (KeyValuePair<int, (string, int)> WSP in model.WorkSpareParts)
            {
                context.WorkSpareParts.Add(new WorkSparePart
                {
                    WorkId = work.Id,
                    SparePartId = WSP.Key,
                    Count = WSP.Value.Item2
                });
                context.SaveChanges();
            }
            return work;
        }

        public WorkViewModel GetElement(WorkBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                var work = context.Works
                    .Include(rec => rec.WorkSpareParts)
                    .ThenInclude(rec => rec.SparePart)
                    .Include(rec => rec.User)
                    .FirstOrDefault(rec => rec.Id == model.Id || rec.WorkName == model.WorkName);
                return work != null ?
                new WorkViewModel
                {
                    Id = work.Id,
                    WorkName = work.WorkName,
                    Price = work.WorkPrice,
                    UserId = work.UserId,
                    UserFIO = work.User.FIO,
                    WorkSpareParts = work.WorkSpareParts.ToDictionary(recWSP => recWSP.SparePartId, recWSP => (recWSP.SparePart?.SparePartName, recWSP.Count))
                } : null;
            }
        }

        public List<WorkViewModel> GetFilteredList(WorkBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new ServiceStationDatabase())
            {
                return context.Works.Include(rec => rec.WorkSpareParts)
                    .ThenInclude(rec => rec.SparePart)
                    .Include(rec => rec.User)
                    .Where(rec => rec.WorkName.Contains(model.WorkName)
                    || model.UserId.HasValue && rec.UserId == model.UserId)
                    .ToList()
                    .Select(rec => new WorkViewModel
                    {
                        Id = rec.Id,
                        WorkName = rec.WorkName,
                        Price = rec.WorkPrice,
                        UserId = rec.UserId,
                        UserFIO = rec.User.FIO,
                        WorkSpareParts = rec.WorkSpareParts.ToDictionary(recWSP => recWSP.SparePartId, recWSP => (recWSP.SparePart?.SparePartName, recWSP.Count))
                    }).ToList();
            }
        }

        public List<WorkViewModel> GetFullList()
        {
            using (var context = new ServiceStationDatabase())
            {
                return context.Works.Include(rec => rec.WorkSpareParts)
                    .ThenInclude(rec => rec.SparePart)
                    .Include(rec => rec.User)
                    .ToList()
                    .Select(rec => new WorkViewModel
                    {
                        Id = rec.Id,
                        WorkName = rec.WorkName,
                        Price = rec.WorkPrice,
                        UserId = rec.UserId,
                        UserFIO = rec.User.FIO,
                        WorkSpareParts = rec.WorkSpareParts.ToDictionary(recWSP => recWSP.SparePartId, recWSP => (recWSP.SparePart?.SparePartName, recWSP.Count))
                    }).ToList();
            }
        }

        public void Insert(WorkBindingModel model)
        {
            using (var context = new ServiceStationDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Work(), context);
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

        public void Update(WorkBindingModel model)
        {
            using (var context = new ServiceStationDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var work = context.Works.FirstOrDefault(rec => rec.Id == model.Id || rec.WorkName == model.WorkName);
                        if (work == null)
                        {
                            throw new Exception("Работа не найдена");
                        }
                        CreateModel(model, work, context);
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

        public void Delete(WorkBindingModel model)
        {
            using (var context = new ServiceStationDatabase())
            {
                var work = context.Works.FirstOrDefault(rec => rec.Id == model.Id || rec.WorkName == model.WorkName);
                if (work != null)
                {
                    context.Works.Remove(work);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Работа не найдена");
                }
            }
        }
    }
}
