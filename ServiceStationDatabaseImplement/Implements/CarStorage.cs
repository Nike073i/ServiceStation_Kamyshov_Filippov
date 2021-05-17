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
    public class CarStorage : ICarStorage
    {
        public Car CreateModel(CarBindingModel model, Car car, ServiceStationDatabase context)
        {
            car.CarName = model.CarName;
            car.UserId = (int) model.UserId;
            if (car.Id == 0)
            {
                context.Cars.Add(car);
                context.SaveChanges();
            }
            if (model.Id.HasValue)
            {
                List<CarSparePart> carSpareParts = context.CarSpareParts
                    .Where(rec => rec.CarId == model.Id.Value)
                    .ToList();
                // удалили те, которых нет в модели
                context.CarSpareParts
                    .RemoveRange(carSpareParts
                    .Where(rec => !model.CarSpareParts.ContainsKey(rec.SparePartId)).ToList());
                context.SaveChanges();
                // Убираем повторы
                foreach (var carSparePart in carSpareParts)
                {
                    if (model.CarSpareParts.ContainsKey(carSparePart.SparePartId))
                    {
                        model.CarSpareParts.Remove(carSparePart.SparePartId);
                    }
                }
                context.SaveChanges();
            }
            // добавили новые
            foreach (KeyValuePair<int, string> CSP in model.CarSpareParts)
            {
                context.CarSpareParts.Add(new CarSparePart
                {
                    CarId = car.Id,
                    SparePartId = CSP.Key
                });
                context.SaveChanges();
            }
            return car;
        }

        public CarViewModel GetElement(CarBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (ServiceStationDatabase context = new ServiceStationDatabase())
            {
                var car = context.Cars.Include(rec => rec.CarSpareParts)
                    .ThenInclude(rec => rec.SparePart)
                    .Include(rec => rec.User)
                    .FirstOrDefault(rec => rec.Id == model.Id || rec.CarName == model.CarName);
                return car != null ?
                new CarViewModel
                {
                    Id = car.Id,
                    CarName = car.CarName,
                    UserId = car.UserId,
                    UserFIO = car.User.FIO,
                    CarSpareParts = car.CarSpareParts.ToDictionary(recCSP => recCSP.SparePartId, recCSP => recCSP.SparePart?.SparePartName)
                } : null;
            }
        }

        public List<CarViewModel> GetFilteredList(CarBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new ServiceStationDatabase())
            {
                return context.Cars.Include(rec => rec.CarSpareParts)
                    .ThenInclude(rec => rec.SparePart)
                    .Include(rec => rec.User)
                    .Where(rec => rec.CarName.Contains(model.CarName) || (model.UserId.HasValue && rec.UserId == model.UserId))
                    .ToList()
                    .Select(rec => new CarViewModel
                    {
                        Id = rec.Id,
                        CarName = rec.CarName,
                        UserId = rec.UserId,
                        UserFIO = rec.User.FIO,
                        CarSpareParts = rec.CarSpareParts.ToDictionary(recCSP => recCSP.SparePartId, recCSP => recCSP.SparePart?.SparePartName)
                    }).ToList();
            }
        }

        public List<CarViewModel> GetFullList()
        {
            using (var context = new ServiceStationDatabase())
            {
                return context.Cars.Include(rec => rec.CarSpareParts)
                    .ThenInclude(rec => rec.SparePart)
                    .Include(rec => rec.User)
                    .ToList()
                    .Select(rec => new CarViewModel
                    {
                        Id = rec.Id,
                        CarName = rec.CarName,
                        UserId = rec.UserId,
                        UserFIO = rec.User.FIO,
                        CarSpareParts = rec.CarSpareParts.ToDictionary(recCSP => recCSP.SparePartId, recCSP => recCSP.SparePart?.SparePartName)
                    }).ToList();
            }
        }

        public void Insert(CarBindingModel model)
        {
            using (var context = new ServiceStationDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Car(), context);
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

        public void Update(CarBindingModel model)
        {
            using (var context = new ServiceStationDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var car = context.Cars.FirstOrDefault(rec => rec.Id == model.Id || rec.CarName == model.CarName);
                        if (car == null)
                        {
                            throw new Exception("Машина не найдена");
                        }
                        CreateModel(model, car, context);
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
        public void Delete(CarBindingModel model)
        {
            using (var context = new ServiceStationDatabase())
            {
                var car = context.Cars.FirstOrDefault(rec => rec.Id == model.Id || rec.CarName == model.CarName);
                if (car != null)
                {
                    context.Cars.Remove(car);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Машина не найдена");
                }
            }
        }
    }
}
