using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.Projects
{
    public class GetProjectsUseCase(IProjectRepository projectsRepository, ICacheService cacheService)
    {
        private const string CacheKey = "projects_cache";

        public async Task<PaginatedResult<Project>> Execute(int page, int pageSize)
        {
            // Definir a chave do cache baseada na página e pageSize
            var cacheKey = $"{CacheKey}_page_{page}_size_{pageSize}";

            // Tentar obter os dados do cache
            var cachedProjects = await cacheService.GetAsync<PaginatedResult<Project>>(cacheKey);

            if (cachedProjects != null)
            {
                return cachedProjects;
            }

            var projects = await projectsRepository.GetPaginatedProjectsAsync(page, pageSize);

            await cacheService.SetAsync(cacheKey, projects, TimeSpan.FromDays(1));

            return projects;
        }
    }
}
