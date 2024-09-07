using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;
using TaskManager.Application.UseCases.Errors

namespace TaskManager.Application.UseCases
{
    public class GetTasksByProjectIdUseCase(ITaskRepository taskRepository, IProjectRepository projectRepository)
    {
        public async Task<PaginatedResult<TaskProject>> Execute(Guid projectId, int page, int pageSize)
        {
           var project = await projectRepository.GetProjectByIdAsync(projectId);

            if(project is null)
            {
                throw new ResourceNotFoundError();
            }

            var tasks = await taskRepository.GetAllTasksByProjectIdAsync(page, pageSize, projectId);

            return tasks;
        }
    }
}
