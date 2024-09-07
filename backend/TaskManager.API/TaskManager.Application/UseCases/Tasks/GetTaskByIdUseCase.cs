
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.UseCases.Errors;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.Tasks
{
    public class GetTaskByIdUseCase(ITaskRepository taskRepository)
    {
        public async Task<TaskProject> Execute(Guid taskId)
        {
            var task = await taskRepository.GetTaskByIdAsync(taskId);

            if (task is null)
            {
                throw new ResourceNotFoundError();
            }

            return task;
        }
    }
}
