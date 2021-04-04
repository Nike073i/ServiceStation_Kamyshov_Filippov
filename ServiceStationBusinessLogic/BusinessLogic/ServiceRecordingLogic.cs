using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.Interfaces;
using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace ServiceStationBusinessLogic.BusinessLogic
{
    public class ServiceRecordingLogic
    {
        private readonly IServiceRecordingStorage _serviceRecordingStorage;
        public ServiceRecordingLogic(IServiceRecordingStorage serviceRecordingStorage)
        {
            _serviceRecordingStorage = serviceRecordingStorage;
        }
        public List<ServiceRecordingViewModel> Read(ServiceRecordingBindingModel model)
        {
            if (model == null)
            {
                return _serviceRecordingStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<ServiceRecordingViewModel> { _serviceRecordingStorage.GetElement(model) };
            }
            return _serviceRecordingStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(ServiceRecordingBindingModel model)
        {
            ServiceRecordingViewModel serviceRecording = _serviceRecordingStorage.GetElement(new ServiceRecordingBindingModel
            {
                DatePassed = model.DatePassed
            });
            if (serviceRecording != null && serviceRecording.Id != model.Id)
            {
                throw new Exception("На это время уже есть запись сервисов");
            }
            if (model.Id.HasValue)
            {
                _serviceRecordingStorage.Update(model);
            }
            else
            {
                _serviceRecordingStorage.Insert(model);
            }
        }
        public void Delete(ServiceRecordingBindingModel model)
        {
            ServiceRecordingViewModel serviceRecording = _serviceRecordingStorage.GetElement(new ServiceRecordingBindingModel
            {
                Id = model.Id
            });
            if (serviceRecording == null)
            {
                throw new Exception("Запись не найдена");
            }
            _serviceRecordingStorage.Delete(model);
        }
    }
}
