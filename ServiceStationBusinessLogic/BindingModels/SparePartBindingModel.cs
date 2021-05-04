namespace ServiceStationBusinessLogic.BindingModels
{
    public class SparePartBindingModel
    {
        public int? Id { get; set; }
        public string SparePartName { get; set; }
        public decimal Price { get; set; }
        public int? UserId { get; set; }
    }
}
