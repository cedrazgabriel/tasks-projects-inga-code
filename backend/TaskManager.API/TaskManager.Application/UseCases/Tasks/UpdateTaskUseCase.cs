using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;
using TaskManager.Application.UseCases.Errors;

namespace TaskManager.Application.UseCases.Tasks
{
    public class UpdateTaskUseCase(ITaskRepository taskRepository, IProjectRepository projectRepository)
    {
        public async Task<TaskProject> Execute(Guid taskId,string name, string description, Guid projectId)
        {
            var project = await projectRepository.GetProjectByIdAsync(projectId);

            if(project is null)
            {
                throw new ResourceNotFoundError();
            }

            var task = await taskRepository.GetTaskByIdAsync(taskId);

            if (task is null)
            {
                throw new ResourceNotFoundError();
            }

            task.Name = name;
            task.Description = description;
            task.ProjectId = projectId;

            await taskRepository.UpdateTaskAsync(task);

            return task;
        }
    }
}
