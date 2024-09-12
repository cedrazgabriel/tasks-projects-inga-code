using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.DTO.Response;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Repositories.Contracts
{
    public interface ITaskRepository
    {
        Task<PaginatedResult<TaskProject>> GetAllTasksAsync(int page, int pageSize);
        Task<PaginatedResult<TaskProject>> GetAllTasksByProjectIdAsync(int page, int pageSize, Guid projectId);
        Task<TaskProject?> GetTaskByIdAsync(Guid id);
        Task CreateTaskAsync(TaskProject task);
        Task UpdateTaskAsync(TaskProject task);
        Task DeleteTaskAsync(TaskProject id);
    }
}
