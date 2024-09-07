namespace TaskManager.API.DTO.Request
{
    public class CreateTaskRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ProjectId { get; set; }
    }
}
