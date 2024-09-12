using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.Services;
using TaskManager.Application.UseCases.Errors;

namespace TaskManager.Application.UseCases.Projects
{
    public class DeleteProjectUseCase(IProjectRepository projectRepository, ICacheService cacheService)
    {
        private const string CacheKey = "projects_cache";

        public async Task<bool> Execute(Guid projectId)
        {
            var project = await projectRepository.GetProjectByIdAsync(projectId);

            if (project is null)
            {
                throw new ResourceNotFoundError();
            }

            project.DeletedAt = DateTime.UtcNow;

            await projectRepository.UpdateAsync(project);

            await cacheService.RemoveByPrefixAsync(CacheKey);

            return true;
        }
    }

}
