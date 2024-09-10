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
            var totalRecords = await dbContext.Tasks.AsNoTracking().CountAsync(); 
            var tasks = await dbContext.Tasks
                .AsNoTracking()
                .Include(task => task.Project)
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
                .Include(task => task.Project)
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

        public async Task<TaskProject> GetTaskByIdAsync(Guid id)
        {
            return await dbContext.Tasks
                .Include(task => task.Project)
                .Include(task => task.TimeTrackers)
                .Where(task => task.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task CreateTaskAsync(TaskProject task)
        {
                await dbContext.Tasks.AddAsync(task);
                await dbContext.SaveChangesAsync();  
        }

        public async Task UpdateTaskAsync(TaskProject task)
        {
            dbContext.Tasks.Update(task);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(TaskProject task)
        {
            dbContext.Tasks.Remove(task);
            await dbContext.SaveChangesAsync();
        }
    }
}
