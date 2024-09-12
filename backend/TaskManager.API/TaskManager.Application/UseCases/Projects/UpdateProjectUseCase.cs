using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.Services;
using TaskManager.Application.UseCases.Errors;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.Projects
{
    public class UpdateProjectUseCase(IProjectRepository projectRepository, ICacheService cacheService)
    {
        private const string CacheKey = "projects_cache";

        public async Task<Project> Execute(Guid projectId, string newName)
        {
            var project = await projectRepository.GetProjectByIdAsync(projectId);

            if (project is null)
            {
                throw new ResourceNotFoundError();
            }

            project.Name = newName;
            project.UpdatedAt = DateTime.UtcNow;

            await projectRepository.UpdateAsync(project);

            await cacheService.RemoveByPrefixAsync(CacheKey);

            return project;
        }
    }
}
