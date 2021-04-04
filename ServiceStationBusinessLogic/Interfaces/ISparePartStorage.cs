using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace ServiceStationBusinessLogic.Interfaces
{
    public interface ISparePartStorage
    {
        List<SparePartViewModel> GetFullList();
        List<SparePartViewModel> GetFilteredList(SparePartBindingModel model);
        SparePartViewModel GetElement(SparePartBindingModel model);
        void Insert(SparePartBindingModel model);
        void Update(SparePartBindingModel model);
        void Delete(SparePartBindingModel model);
    }
}
