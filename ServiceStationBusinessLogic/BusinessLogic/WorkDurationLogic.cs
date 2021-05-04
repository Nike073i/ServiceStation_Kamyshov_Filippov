using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.Interfaces;
using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace ServiceStationBusinessLogic.BusinessLogic
{
    public class WorkDurationLogic
    {
        private readonly IWorkDurationStorage _workDurationStorage;
        public WorkDurationLogic(IWorkDurationStorage workDurationStorage)
        {
            _workDurationStorage = workDurationStorage;
        }
        public List<WorkDurationViewModel> Read(WorkDurationBindingModel model)
        {
            if (model == null)
            {
                return _workDurationStorage.GetFullList();
            }
            if (model.TimeFrom.HasValue || model.TimeTo.HasValue || model.UserId.HasValue)
            {
                return _workDurationStorage.GetFilteredList(model);
            }
            return new List<WorkDurationViewModel> { _workDurationStorage.GetElement(model) };
        }
        public void CreateOrUpdate(WorkDurationBindingModel model)
        {
            var workDuration = _workDurationStorage.GetElement(new WorkDurationBindingModel
            {
                WorkId = model.WorkId
            });
            if (workDuration != null)
            {
                _workDurationStorage.Update(model);
            }
            else
            {
                _workDurationStorage.Insert(model);
            }
        }
        public void Delete(WorkDurationBindingModel model)
        {
            var workDuration = _workDurationStorage.GetElement(new WorkDurationBindingModel
            {
                WorkId = model.WorkId
            });
            if (workDuration == null)
            {
                throw new Exception("Продолжительность работы не найдена");
            }
            _workDurationStorage.Delete(model);
        }
    }
}
