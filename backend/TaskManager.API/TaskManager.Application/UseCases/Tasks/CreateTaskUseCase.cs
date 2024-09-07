using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;
using TaskManager.Application.UseCases.Errors;


namespace TaskManager.Application.UseCases.Tasks
{
    public class CreateTaskUseCase(ITaskRepository taskRepository, IProjectRepository projectRepository)
    {
        public async Task<TaskProject> Execute(string name, string description, Guid projectId)
        {
            var project = await projectRepository.GetProjectByIdAsync(projectId);

            if (project is null)
            {
                throw new ResourceNotFoundError();
            }

            var task = new TaskProject(name, description, projectId);

            await taskRepository.CreateTaskAsync(task);

            return task;
        }
    }
}
