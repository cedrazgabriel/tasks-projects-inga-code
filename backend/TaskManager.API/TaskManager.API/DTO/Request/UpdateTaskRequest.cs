﻿namespace TaskManager.API.DTO.Request
{
    public class UpdateTaskRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ProjectId { get; set; }
    }
}
