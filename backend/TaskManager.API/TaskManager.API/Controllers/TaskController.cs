using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.UseCases;
using TaskManager.Infrastructure.Persistence.Repositories;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController(ITaskRepository taskRepository) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResult<TaskResponse>))]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Consultar tasks", Description = "Este endpoint é usado para consultar todas as tasks da plataforma.")]
        public async Task<ActionResult<PaginatedResult<TaskResponse>>> GetTasks([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var useCase = new GetTasksUseCase(taskRepository);

            var tasks = await useCase.Execute(page, pageSize);

            var response = new PaginatedResult<TaskResponse>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = tasks.TotalRecords,
                Items = tasks.Items.Select(task => new TaskResponse
                {
                    Id = task.Id.ToString(),
                    Name = task.Name,
                    Description = task.Description,
                    ProjectId = task.ProjectId.ToString(),
                    CreatedAt = task.CreatedAt.ToString("yyyy-mm-dd HH:mm:ss")
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet("byProject/{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResult<TaskResponse>))]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Consultar tasks por ProjectId", Description = "Este endpoint é usado para consultar todas as tasks associadas a um projeto.")]
        public async Task<ActionResult<PaginatedResult<TaskResponse>>> GetTasksByProjectId(Guid projectId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var useCase = new GetTasksByProjectIdUseCase(taskRepository);

            var tasks = await useCase.Execute(projectId, page, pageSize);

            var response = new PaginatedResult<TaskResponse>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = tasks.TotalRecords,
                Items = tasks.Items.Select(task => new TaskResponse
                {
                    Id = task.Id.ToString(),
                    Name = task.Name,
                    Description = task.Description,
                    ProjectId = task.ProjectId.ToString(),
                    CreatedAt = task.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                }).ToList()
            };

            return Ok(response);
        }
    }
}
