using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.Interfaces;
using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace ServiceStationBusinessLogic.BusinessLogic
{
    public class TechnicalMaintenanceLogic
    {
        private readonly ITechnicalMaintenanceStorage _technicalMaintenanceStorage;
        public TechnicalMaintenanceLogic(ITechnicalMaintenanceStorage technicalMaintenanceStorage)
        {
            _technicalMaintenanceStorage = technicalMaintenanceStorage;
        }
        public List<TechnicalMaintenanceViewModel> Read(TechnicalMaintenanceBindingModel model)
        {
            if (model == null)
            {
                return _technicalMaintenanceStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<TechnicalMaintenanceViewModel> { _technicalMaintenanceStorage.GetElement(model) };
            }
            return _technicalMaintenanceStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(TechnicalMaintenanceBindingModel model)
        {
            TechnicalMaintenanceViewModel technicalMaintenance = _technicalMaintenanceStorage.GetElement(new TechnicalMaintenanceBindingModel
            {
                TechnicalMaintenanceName = model.TechnicalMaintenanceName
            });
            if (technicalMaintenance != null && technicalMaintenance.Id != model.Id)
            {
                throw new Exception("Уже есть ТО с таким названием");
            }
            if (model.Id.HasValue)
            {
                _technicalMaintenanceStorage.Update(model);
            }
            else
            {
                _technicalMaintenanceStorage.Insert(model);
            }
        }
        public void Delete(TechnicalMaintenanceBindingModel model)
        {
            TechnicalMaintenanceViewModel technicalMaintenance = _technicalMaintenanceStorage.GetElement(new TechnicalMaintenanceBindingModel
            {
                Id = model.Id
            });
            if (technicalMaintenance == null)
            {
                throw new Exception("ТО не найдено");
            }
            _technicalMaintenanceStorage.Delete(model);
        }
    }
}
