namespace TaskManager.API.DTO.Response
{
    public class TimeTrackerResponse
    {
        public string Id { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string? EndDate { get; set; } = string.Empty;
        public string CollaboratorId { get; set; } = string.Empty;
        public string CollaboratorName { get; set; } = string.Empty ;
        public string CreatedAt { get; set; } = string.Empty;
        public string TaskName { get; set; } = string.Empty;
        public string TaskId { get; set; } = string.Empty;
        public string? UpdatedAt { get; set; }
        public string ProjectId { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
    }
}
