using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.Interfaces;
using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace ServiceStationBusinessLogic.BusinessLogic
{
    public class SparePartLogic
    {
        private readonly ISparePartStorage _sparePartStorage;
        public SparePartLogic(ISparePartStorage sparePartStorage)
        {
            _sparePartStorage = sparePartStorage;
        }
        public List<SparePartViewModel> Read(SparePartBindingModel model)
        {
            if (model == null)
            {
                return _sparePartStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<SparePartViewModel> { _sparePartStorage.GetElement(model) };
            }
            return _sparePartStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(SparePartBindingModel model)
        {
            var sparePart = _sparePartStorage.GetElement(new SparePartBindingModel
            {
                SparePartName = model.SparePartName
            });
            if (sparePart != null && sparePart.Id != model.Id)
            {
                throw new Exception("Уже есть запчасть с таким названием");
            }
            if (model.Id.HasValue)
            {
                _sparePartStorage.Update(model);
            }
            else
            {
                _sparePartStorage.Insert(model);
            }
        }
        public void Delete(SparePartBindingModel model)
        {
            var sparePart = _sparePartStorage.GetElement(new SparePartBindingModel
            {
                Id = model.Id
            });
            if (sparePart == null)
            {
                throw new Exception("Запчасть не найдена");
            }
            _sparePartStorage.Delete(model);
        }
    }
}
