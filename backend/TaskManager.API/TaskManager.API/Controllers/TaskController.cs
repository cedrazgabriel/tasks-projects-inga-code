using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TaskManager.API.DTO.Request;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.UseCases;
using TaskManager.Application.UseCases.Projects;
using TaskManager.Application.UseCases.Tasks;
using TaskManager.Infrastructure.Persistence.Repositories;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController(ITaskRepository taskRepository, IProjectRepository projectRepository) : ControllerBase
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Consultar tasks por ProjectId", Description = "Este endpoint é usado para consultar todas as tasks associadas a um projeto.")]
        public async Task<ActionResult<PaginatedResult<TaskResponse>>> GetTasksByProjectId(Guid projectId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var useCase = new GetTasksByProjectIdUseCase(taskRepository, projectRepository);

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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Consultar task por id", Description = "Este endpoint é usado para consultar uma task específica por id.")]
        public async Task<ActionResult<TaskResponse>> GetTaskById(Guid id)
        {
            var useCase = new GetTaskByIdUseCase(taskRepository);

            var task = await useCase.Execute(id);

            var response = new TaskResponse
            {
                Id = task.Id.ToString(),
                Name = task.Name,
                Description = task.Description,
                ProjectId = task.ProjectId.ToString(),
                CreatedAt = task.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return Ok(response);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TaskResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Criar uma task", Description = "Este endpoint é usado para criar uma task.")]
        public async Task<ActionResult<TaskResponse>> CreateTask([FromBody] CreateTaskRequest request)
        {
            var useCase = new CreateTaskUseCase(taskRepository, projectRepository);

            var task = await useCase.Execute(request.Name, request.Description, request.ProjectId);

            var response = new TaskResponse
            {
                Id = task.Id.ToString(),
                Name = task.Name,
                Description = task.Description,
                ProjectId = task.ProjectId.ToString(),
                CreatedAt = task.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return CreatedAtAction(nameof(CreateTask), new { id = task.Id }, response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Atualizar task", Description = "Este endpoint é usado para atualizar uma task existente na plataforma.")]
        public async Task<ActionResult<TaskResponse>> UpdateTask(Guid id, [FromBody] UpdateTaskRequest request)
        {
            if (id == Guid.Empty || !Guid.TryParse(id.ToString(), out var projectId))
            {
                return BadRequest("The ID provided is not valid.");
            }

            var useCase = new UpdateTaskUseCase(taskRepository, projectRepository);

            var task = await useCase.Execute(id, request.Name, request.Description, request.ProjectId);

            var response = new TaskResponse
            {
                Id = task.Id.ToString(),
                Name = task.Name,
                Description = task.Description,
                CreatedAt = task.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                UpdatedAt = task.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Deletar task", Description = "Este endpoint é usado para deletar uma task específica da plataforma.")]
        public async Task<ActionResult<BasicResponse>> DeleteTask(Guid id)
        {
            if (id == Guid.Empty || !Guid.TryParse(id.ToString(), out var projectId))
            {
                return BadRequest("The ID provided is not valid.");
            }

            var useCase = new DeleteTaskUseCase(taskRepository);

            await useCase.Execute(projectId);

            var response = new BasicResponse()
            {
                message = "Task deleted successfully."
            };

            return Ok(response);
        }
    }
}
