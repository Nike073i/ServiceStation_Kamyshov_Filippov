using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.Interfaces;
using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace ServiceStationBusinessLogic.BusinessLogic
{
    public class UserLogic
    {
        private readonly IUserStorage _userStorage;
        public UserLogic(IUserStorage userStorage)
        {
            _userStorage = userStorage;
        }
        public List<UserViewModel> Read(UserBindingModel model)
        {
            if (model == null)
            {
                return _userStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<UserViewModel> { _userStorage.GetElement(model) };
            }
            return _userStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(UserBindingModel model)
        {
            var user = _userStorage.GetElement(new UserBindingModel
            {
                Email = model.Email
            });
            if (user != null && user.Id != model.Id)
            {
                throw new Exception("Уже есть пользователь с такой почтой");
            }
            if (model.Id.HasValue)
            {
                _userStorage.Update(model);
            }
            else
            {
                _userStorage.Insert(model);
            }
        }
        public void Delete(UserBindingModel model)
        {
            var user = _userStorage.GetElement(new UserBindingModel
            {
                Id = model.Id
            });
            if (user == null)
            {
                throw new Exception("Пользователь не найден");
            }
            _userStorage.Delete(model);
        }
        public void ChangePosition(ChangeUserPositionBindingModel model)
        {
            var user = _userStorage.GetElement(new UserBindingModel
            {
                Id = model.UserId
            });
            if (user == null)
            {
                throw new Exception("Пользователь не найден");
            }
            _userStorage.Update(new UserBindingModel
            {
                Id = user.Id,
                Email = user.Email,
                FIO = user.FIO,
                Password = user.Password,
                Position = model.Position
            });
        }
    }
}
