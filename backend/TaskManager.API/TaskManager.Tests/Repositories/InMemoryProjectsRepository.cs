using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.API.DTO.Response;
using TaskManager.Domain.Entities;

namespace TaskManager.Tests.Repositories
{
    public class InMemoryProjectsRepository : IProjectRepository
    {
        public List<Project> projects;

        public InMemoryProjectsRepository()
        {
            projects = new List<Project>();
        }

        public Task CreateAsync(Project project)
        {
            projects.Add(project);
            return Task.CompletedTask;
        }

        public Task<PaginatedResult<Project>> GetPaginatedProjectsAsync(int page, int pageSize)
        {
            var paginatedProjects = projects
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PaginatedResult<Project>
            {
                Items = paginatedProjects,
                Page = page,
                PageSize = pageSize,
                TotalRecords = projects.Count
            };

            return Task.FromResult(result);
        }

        public Task<Project?> GetProjectByIdAsync(Guid id)
        {
            var project = projects.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(project);
        }

        public Task UpdateAsync(Project project)
        {
            var existingProject = projects.FirstOrDefault(p => p.Id == project.Id);
            if (existingProject != null)
            {
                projects.Remove(existingProject);
                projects.Add(project);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Project project)
        {
            projects.Remove(project);
            return Task.CompletedTask;
        }
    }
}
