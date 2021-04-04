using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.Interfaces;
using ServiceStationBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace ServiceStationBusinessLogic.BusinessLogic
{
    public class WorkLogic
    {
        private readonly IWorkStorage _workStorage;
        public WorkLogic(IWorkStorage workStorage)
        {
            _workStorage = workStorage;
        }
        public List<WorkViewModel> Read(WorkBindingModel model)
        {
            if (model == null)
            {
                return _workStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<WorkViewModel> { _workStorage.GetElement(model) };
            }
            return _workStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(WorkBindingModel model)
        {
            var work = _workStorage.GetElement(new WorkBindingModel
            {
                WorkName = model.WorkName
            });
            if (work != null && work.Id != model.Id)
            {
                throw new Exception("Уже есть работа с таким названием");
            }
            if (model.Id.HasValue)
            {
                _workStorage.Update(model);
            }
            else
            {
                _workStorage.Insert(model);
            }
        }
        public void Delete(WorkBindingModel model)
        {
            var work = _workStorage.GetElement(new WorkBindingModel
            {
                Id = model.Id
            });
            if (work == null)
            {
                throw new Exception("Работа не найдена");
            }
            _workStorage.Delete(model);
        }
    }
}
