namespace TaskManager.API.DTO.Response
{
    public class ProjectResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty ;
        public string CreatedAt { get; set; } = string.Empty;
        public string? UpdatedAt { get; set; } = null;
    }
}
