using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace ServiceStationBusinessLogic.Interfaces
{
    public interface ITechnicalMaintenanceStorage
    {
        List<TechnicalMaintenanceViewModel> GetFullList();
        List<TechnicalMaintenanceViewModel> GetFilteredList(TechnicalMaintenanceBindingModel model);
        TechnicalMaintenanceViewModel GetElement(TechnicalMaintenanceBindingModel model);
        void Insert(TechnicalMaintenanceBindingModel model);
        void Update(TechnicalMaintenanceBindingModel model);
        void Delete(TechnicalMaintenanceBindingModel model);
    }
}
