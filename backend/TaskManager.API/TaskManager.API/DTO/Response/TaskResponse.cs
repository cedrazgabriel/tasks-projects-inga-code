namespace TaskManager.API.DTO.Response
{
    public class TaskResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty ;
        public string Description { get; set; } = string.Empty;
        public string ProjectId { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
        public string? UpdatedAt { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string TotalTimeSpent { get; set; } = string.Empty;
    }
}
