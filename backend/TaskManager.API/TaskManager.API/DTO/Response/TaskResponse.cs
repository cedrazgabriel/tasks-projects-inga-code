namespace TaskManager.API.DTO.Response
{
    public class TaskResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProjectId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string ProjectName { get; set; }
        public string TotalTimeSpent { get; set; }
    }
}
