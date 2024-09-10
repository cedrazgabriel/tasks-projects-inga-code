namespace TaskManager.API.DTO.Request
{
    public class InitTimeTrackerRequest
    {
        public string StartDateTime { get; set; }
        public string TimeZoneId { get; set; }
        public Guid TaskId { get; set; }
    }
}
