﻿namespace TaskManager.API.DTO.Request
{
    public class CreateTaskRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid ProjectId { get; set; } 
    }
}
