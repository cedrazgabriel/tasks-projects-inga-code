using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.DTO.Response;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Repositories.Contracts
{
    public interface IProjectRepository
    {
        public Task CreateAsync(Project project);
        public Task<PaginatedResult<Project>> GetPaginatedProjectsAsync(int page, int pageSize);
        public Task<Project?> GetProjectByIdAsync(Guid id);
        public Task UpdateAsync(Project project);
        public Task DeleteAsync(Project projectId);
    }
}
