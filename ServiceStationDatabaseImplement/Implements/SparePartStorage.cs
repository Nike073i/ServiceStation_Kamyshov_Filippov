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
    public class SparePartStorage : ISparePartStorage
    {
        public SparePartViewModel GetElement(SparePartBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                SparePart sparePart = context.SpareParts
                    .Include(rec => rec.User)
                    .FirstOrDefault(rec => rec.Id == model.Id || rec.SparePartName == model.SparePartName);
                return sparePart != null ?
                new SparePartViewModel
                {
                    Id = sparePart.Id,
                    SparePartName = sparePart.SparePartName,
                    Price = sparePart.SparePartPrice,
                    UserId = sparePart.UserId,
                    UserFIO = sparePart.User.FIO
                } : null;
            }
        }

        public List<SparePartViewModel> GetFilteredList(SparePartBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                return context.SpareParts
                    .Include(rec => rec.User)
                    .Where(rec => rec.SparePartName.Contains(model.SparePartName)
                    || (model.UserId.HasValue && rec.UserId == model.UserId))
                    .Select(rec => new SparePartViewModel
                    {
                        Id = rec.Id,
                        SparePartName = rec.SparePartName,
                        Price = rec.SparePartPrice,
                        UserId = rec.UserId,
                        UserFIO = rec.User.FIO
                    }).ToList();
            }
        }

        public List<SparePartViewModel> GetFullList()
        {
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                return context.SpareParts
                    .Include(rec => rec.User)
                    .Select(rec => new SparePartViewModel
                    {
                        Id = rec.Id,
                        SparePartName = rec.SparePartName,
                        Price = rec.SparePartPrice,
                        UserId = rec.UserId,
                        UserFIO = rec.User.FIO
                    }).ToList();
            }
        }

        public void Insert(SparePartBindingModel model)
        {
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                context.SpareParts.Add(CreateModel(model, new SparePart()));
                context.SaveChanges();
            }
        }

        public void Update(SparePartBindingModel model)
        {
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                SparePart sparePart = context.SpareParts.FirstOrDefault(rec => rec.Id == model.Id);
                if (sparePart == null)
                {
                    throw new Exception("Запчасть не найдена");
                }
                CreateModel(model, sparePart);
                context.SaveChanges();
            }
        }

        public void Delete(SparePartBindingModel model)
        {
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                SparePart sparePart = context.SpareParts.FirstOrDefault(rec => rec.Id == model.Id);
                if (sparePart != null)
                {
                    context.SpareParts.Remove(sparePart);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Запчасть не найдена");
                }
            }
        }

        public SparePart CreateModel(SparePartBindingModel model, SparePart sparePart)
        {
            sparePart.SparePartName = model.SparePartName;
            sparePart.SparePartPrice = model.Price;
            sparePart.UserId = (int)model.UserId;
            return sparePart;
        }
    }
}
