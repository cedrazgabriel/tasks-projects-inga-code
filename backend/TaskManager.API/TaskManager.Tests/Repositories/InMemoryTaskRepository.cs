using TaskManager.API.DTO.Response;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;

namespace TaskManager.Tests.Repositories
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        // Propriedade pública Items que armazena as tasks em memória
        public List<TaskProject> Items { get; private set; } = new List<TaskProject>();

        public Task<PaginatedResult<TaskProject>> GetAllTasksAsync(int page, int pageSize)
        {
            var paginatedItems = Items
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PaginatedResult<TaskProject>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = Items.Count,
                Items = paginatedItems
            };

            return Task.FromResult(result);
        }

        public Task<PaginatedResult<TaskProject>> GetAllTasksByProjectIdAsync(int page, int pageSize, Guid projectId)
        {
            var paginatedItems = Items
                .Where(task => task.ProjectId == projectId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PaginatedResult<TaskProject>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = Items.Count(task => task.ProjectId == projectId),
                Items = paginatedItems
            };

            return Task.FromResult(result);
        }

        public Task<TaskProject?> GetTaskByIdAsync(Guid id)
        {
            var task = Items.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(task);
        }

        public Task CreateTaskAsync(TaskProject task)
        {
            Items.Add(task);
            return Task.CompletedTask;
        }

        public Task UpdateTaskAsync(TaskProject task)
        {
            var existingTask = Items.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask != null)
            {
                // Atualizar os campos necessários
                existingTask.Name = task.Name;
                existingTask.Description = task.Description;
                existingTask.ProjectId = task.ProjectId;
                existingTask.UpdatedAt = task.UpdatedAt;
            }

            return Task.CompletedTask;
        }

        public Task DeleteTaskAsync(TaskProject task)
        {
            Items.Remove(task);
            return Task.CompletedTask;
        }
    }
}
