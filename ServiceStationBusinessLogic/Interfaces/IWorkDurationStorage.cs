using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace ServiceStationBusinessLogic.Interfaces
{
    public interface IWorkDurationStorage
    {
        List<WorkDurationViewModel> GetFullList();
        List<WorkDurationViewModel> GetFilteredList(WorkDurationBindingModel model);
        WorkDurationViewModel GetElement(WorkDurationBindingModel model);
        void Insert(WorkDurationBindingModel model);
        void Update(WorkDurationBindingModel model);
        void Delete(WorkDurationBindingModel model);
    }
}
