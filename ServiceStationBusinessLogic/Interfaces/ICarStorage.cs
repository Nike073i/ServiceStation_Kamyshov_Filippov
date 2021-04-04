using ServiceStationBusinessLogic.BindingModels;
using ServiceStationBusinessLogic.ViewModels;
using System.Collections.Generic;


namespace ServiceStationBusinessLogic.Interfaces
{
    public interface ICarStorage
    {
        List<CarViewModel> GetFullList();
        List<CarViewModel> GetFilteredList(CarBindingModel model);
        CarViewModel GetElement(CarBindingModel model);
        void Insert(CarBindingModel model);
        void Update(CarBindingModel model);
        void Delete(CarBindingModel model);
    }
}
