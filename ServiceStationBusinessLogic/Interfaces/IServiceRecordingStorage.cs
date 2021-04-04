using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace ServiceStationBusinessLogic.Interfaces
{
    public interface IServiceRecordingStorage
    {
        List<ServiceRecordingViewModel> GetFullList();
        List<ServiceRecordingViewModel> GetFilteredList(ServiceRecordingBindingModel model);
        ServiceRecordingViewModel GetElement(ServiceRecordingBindingModel model);
        void Insert(ServiceRecordingBindingModel model);
        void Update(ServiceRecordingBindingModel model);
        void Delete(ServiceRecordingBindingModel model);
    }
}
