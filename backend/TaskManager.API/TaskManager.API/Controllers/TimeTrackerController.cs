using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.API.DTO.Request;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.UseCases.Tasks;
using TaskManager.Application.UseCases.TimeTrackers;
using TaskManager.Infrastructure.Persistence.Repositories;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [SwaggerTag("Gerencia os tempos relacionados a uma task da aplicação")]
    public class TimeTrackerController(ITaskRepository taskRepository, ITimeTrackerRepository timeTrackerRepository, ICollaboratorRepository collaboratorRepository) : ControllerBase
    {
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TaskResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Iniciar time tracker", Description = "Inicia um time tracker de uma task específica.")]
        public async Task<ActionResult<TimeTrackerResponse>> Create([FromBody] InitTimeTrackerRequest request)
        {
            var useCase = new InitTimeTrackerUseCase(taskRepository, timeTrackerRepository, collaboratorRepository);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Usuário não autenticado.");
            }

            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return BadRequest("ID de usuário inválido.");
            }

            var timeTracker = await useCase.Execute(userGuid,
                request.TaskId,
                Convert.ToDateTime(request.StartDateTime),
                request.TimeZoneId);

            var response = new TimeTrackerResponse
            {
               CollaboratorId = timeTracker.CollaboratorId.ToString(),
               CollaboratorName = timeTracker.Collaborator.Name,
               CreatedAt = timeTracker.CreatedAt.ToString("yyyy/MM/dd HH:mm:ss"),
               EndDate = null,
               Id = timeTracker.Id.ToString(),
               StartDate = timeTracker.StartDate.ToString("yyyy/MM/dd HH:mm:ss"),
               UpdatedAt = timeTracker.UpdatedAt?.ToString("yyyy/MM/dd HH:mm:ss")
            };

            return CreatedAtAction(nameof(Create), new { id = timeTracker.Id }, response);
        }

        [HttpGet("{taskId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TaskResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Retorna todas as tasks com a opção de filtrar os time trackers por tasks.")]
        public async Task<ActionResult<PaginatedResult<TimeTrackerResponse>>> GetTimeTrackersByTaskId(Guid taskId, [FromQuery] int page = 1,[FromQuery] int pageSize = 10)
        {

            var useCase = new GetTimeTrackersByTaskIdUseCase(timeTrackerRepository);

            var tasks = await useCase.Execute(taskId, page, pageSize);

            var response = new PaginatedResult<TimeTrackerResponse>()
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = tasks.TotalRecords,
                Items = tasks.Items.Select(tt => new TimeTrackerResponse()
                {
                    Id = tt.Id.ToString(),
                    CreatedAt = tt.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    CollaboratorId = tt.CollaboratorId.ToString(),
                    CollaboratorName = tt.Collaborator.Name,
                    EndDate = tt.EndDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                    StartDate = tt.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    TaskId = tt.TaskId.ToString(),
                    TaskName = tt.Task.Name,
                    UpdatedAt = tt.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
                    ProjectId = tt.Task.Project.Id.ToString(),
                    ProjectName = tt.Task.Project.Name
                }).ToList()
            };
                
             
            return Ok(response);
        }
    }
}
