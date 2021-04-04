using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.Interfaces;
using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace ServiceStationBusinessLogic.BusinessLogic
{
    public class CarLogic
    {
        private readonly ICarStorage _carStorage;
        public CarLogic(ICarStorage carStorage)
        {
            _carStorage = carStorage;
        }
        public List<CarViewModel> Read(CarBindingModel model)
        {
            if (model == null)
            {
                return _carStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<CarViewModel> { _carStorage.GetElement(model) };
            }
            return _carStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(CarBindingModel model)
        {
            CarViewModel car = _carStorage.GetElement(new CarBindingModel
            {
                CarName = model.CarName
            });
            if (car != null && car.Id != model.Id)
            {
                throw new Exception("Уже есть машина с таким названием");
            }
            if (model.Id.HasValue)
            {
                _carStorage.Update(model);
            }
            else
            {
                _carStorage.Insert(model);
            }
        }
        public void Delete(CarBindingModel model)
        {
            CarViewModel car = _carStorage.GetElement(new CarBindingModel
            {
                Id = model.Id
            });
            if (car == null)
            {
                throw new Exception("Машина не найдена");
            }
            _carStorage.Delete(model);
        }
    }
}
