namespace TaskManager.API.DTO.Request
{
    public class InitTimeTrackerRequest
    {
        public string TimeZoneId { get; set; } = string.Empty;
        public Guid TaskId { get; set; }
    }
}
