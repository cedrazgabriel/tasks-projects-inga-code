using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.Repositories
{
    public class TaskRepository(TaskManagerDbContext dbContext) : ITaskRepository
    {
        public async Task<PaginatedResult<TaskProject>> GetAllTasksAsync(int page, int pageSize)
        {
            var totalRecords = await dbContext.Tasks.AsNoTracking().CountAsync(); // Adiciona o AsNoTracking na contagem
            var tasks = await dbContext.Tasks
                .AsNoTracking() 
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<TaskProject>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Items = tasks
            };
        }

        public async Task<PaginatedResult<TaskProject>> GetAllTasksByProjectIdAsync(int page, int pageSize, Guid projectId)
        {
            var totalRecords = await dbContext.Tasks
                .AsNoTracking() 
                .Where(task => task.ProjectId == projectId)
                .CountAsync();

            var tasks = await dbContext.Tasks
                .AsNoTracking() 
                .Where(task => task.ProjectId == projectId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<TaskProject>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Items = tasks
            };
        }
    }
}
