using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.Projects
{
    public class CreateProjectUseCase(IProjectRepository projectRepository, ICacheService cacheService)
    {
        private const string CacheKey = "projects_cache";

        public async Task<Project> Execute(string name)
        {
            var project = new Project(name);
            project.CreatedAt = DateTime.UtcNow;

            await projectRepository.CreateAsync(project);

            await cacheService.RemoveByPrefixAsync(CacheKey);

            return project;
        }
    }
}
