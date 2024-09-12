namespace TaskManager.API.DTO.Request
{
    public class InitTimeTrackerRequest
    {
        public string TimeZoneId { get; set; }
        public Guid TaskId { get; set; }
    }
}
