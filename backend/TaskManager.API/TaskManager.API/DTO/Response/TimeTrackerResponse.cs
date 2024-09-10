namespace TaskManager.API.DTO.Response
{
    public class TimeTrackerResponse
    {
        public string Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CollaboratorId { get; set; }
        public string CollaboratorName { get; set; }
        public string CreatedAt { get; set; }
        public string TaskName { get; set; }
        public string TaskId { get; set; }
        public string UpdatedAt { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
