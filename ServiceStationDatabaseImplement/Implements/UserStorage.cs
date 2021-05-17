using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.Interfaces;
using ServiceStationBusinessLogic.ViewModels;
using ServiceStationDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceStationDatabaseImplement.Implements
{
    public class UserStorage : IUserStorage
    {
        public List<UserViewModel> GetFullList()
        {
            using (var context = new ServiceStationDatabase())
            {
                return context.Users
                .Select(rec => new UserViewModel
                {
                    Id = rec.Id,
                    FIO = rec.FIO,
                    Email = rec.Email,
                    Password = rec.Password,
                    Position = rec.Position,
                }).ToList();
            }
        }
        public List<UserViewModel> GetFilteredList(UserBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new ServiceStationDatabase())
            {
                return context.Users
                .Where(rec => rec.Email.Equals(model.Email) && rec.Password.Equals(model.Password))
                .Select(rec => new UserViewModel
                {
                    Id = rec.Id,
                    FIO = rec.FIO,
                    Email = rec.Email,
                    Password = rec.Password,
                    Position = rec.Position,
                }).ToList();
            }
        }
        public UserViewModel GetElement(UserBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new ServiceStationDatabase())
            {
                var user = context.Users
                .FirstOrDefault(rec => rec.Id == model.Id || rec.Email == model.Email);
                return user != null ?
                new UserViewModel
                {
                    Id = user.Id,
                    FIO = user.FIO,
                    Email = user.Email,
                    Password = user.Password,
                    Position = user.Position,
                } : null;
            }
        }
        public void Insert(UserBindingModel model)
        {
            using (var context = new ServiceStationDatabase())
            {
                context.Users.Add(CreateModel(model, new User()));
                context.SaveChanges();
            }
        }
        public void Update(UserBindingModel model)
        {
            using (var context = new ServiceStationDatabase())
            {
                var user = context.Users.FirstOrDefault(rec => rec.Id == model.Id);
                if (user == null)
                {
                    throw new Exception("Пользователь не найден");
                }
                CreateModel(model, user);
                context.SaveChanges();
            }
        }
        public void Delete(UserBindingModel model)
        {
            using (var context = new ServiceStationDatabase())
            {
                User user = context.Users.FirstOrDefault(rec => rec.Id == model.Id);
                if (user != null)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Пользователь не найден");
                }
            }
        }
        private User CreateModel(UserBindingModel model, User user)
        {
            user.FIO = model.FIO;
            user.Email = model.Email;
            user.Password = model.Password;
            user.Position = model.Position;
            return user;
        }
    }
}
