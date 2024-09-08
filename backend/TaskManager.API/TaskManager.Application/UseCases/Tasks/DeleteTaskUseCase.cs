using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.UseCases.Errors;

namespace TaskManager.Application.UseCases.Tasks
{
    public class DeleteTaskUseCase(ITaskRepository taskRepository)
    {
        public async Task Execute(Guid taskId)
        {
            var task = await taskRepository.GetTaskByIdAsync(taskId);

            if(task is null)
            {
                throw new ResourceNotFoundError();
            }

            task.DeletedAt = DateTime.UtcNow;

            await taskRepository.UpdateTaskAsync(task);
        }
    }
}
