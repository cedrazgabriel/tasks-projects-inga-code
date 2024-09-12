namespace TaskManager.API.DTO.Response
{
    public class MetricsResponse
    {
        public string TotalHoursSpentThisMonth { get; set; } = string.Empty;
        public string TotalHoursSpentToday {  get; set; } = string.Empty ;
        public string TotalHoursSpentThisWeek { get; set; } = string.Empty;
    }
}
