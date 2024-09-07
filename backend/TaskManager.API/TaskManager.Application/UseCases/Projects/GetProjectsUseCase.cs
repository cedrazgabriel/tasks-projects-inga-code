using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.Projects
{
    public class GetProjectsUseCase(IProjectRepository projectsRepository)
    {
        public async Task<PaginatedResult<Project>> Execute(int page, int pageSize)
        {
            return await projectsRepository.GetPaginatedProjectsAsync(page, pageSize);
        }
    }
}
