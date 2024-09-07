using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;


namespace TaskManager.Application.UseCases.Tasks
{
    public class GetTasksUseCase(ITaskRepository tasksRepository)
    {
        public async Task<PaginatedResult<TaskProject>> Execute(int page, int pageSize)
        {
            return await tasksRepository.GetAllTasksAsync(page, pageSize);
        }
    }
}
