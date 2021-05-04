namespace ServiceStationBusinessLogic.BindingModels
{
    public class WorkDurationBindingModel
    {
        public int WorkId { get; set; }
        public int Duration { get; set; }
        public int? UserId { get; set; }
        public int? TimeFrom { get; set; }
        public int? TimeTo { get; set; }
    }
}
