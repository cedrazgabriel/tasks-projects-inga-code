using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.Repositories
{
    public class ProjectRepository(TaskManagerDbContext dbContext) : IProjectRepository
    {
        public async Task CreateAsync(Project project)
        {
            await dbContext.Projects.AddAsync(project);
            await dbContext.SaveChangesAsync();
        }

        public async Task<PaginatedResult<Project>> GetPaginatedProjectsAsync(int page, int pageSize)
        {
            var totalRecords = await dbContext.Projects.CountAsync();
            var projects = await dbContext.Projects
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Project>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Items = projects
            };
        }

        public Task<Project> GetProjectByIdAsync(Guid id)
        {
            return dbContext.Projects.FirstOrDefaultAsync(project => project.Id == id);
        }

        public async Task UpdateAsync(Project project)
        {
            dbContext.Projects.Update(project);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Project project)
        {
            dbContext.Projects.Remove(project);
            await dbContext.SaveChangesAsync();
        }
    }
}
