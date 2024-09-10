using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
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
            var useCase = new CreateTimeTrackerUseCase(taskRepository, timeTrackerRepository, collaboratorRepository);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Usuário não autenticado.");
            }

            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return BadRequest("ID de usuário inválido.");
            }

            var timeTracker = await useCase.a(userGuid,
                request.TaskId,
                Convert.ToDateTime(request.StartDateTime),
                Convert.ToDateTime(request.EndDateTime),
                request.TimeZoneId);

            var response = new TimeTrackerResponse
            {
               CollaboratorId = timeTracker.CollaboratorId.ToString(),
               CollaboratorName = timeTracker.Collaborator.Name,
               CreatedAt = timeTracker.CreatedAt.ToString("yyyy/MM/dd HH:mm:ss"),
               EndDate = timeTracker.EndDate.ToString("yyyy/MM/dd HH:mm:ss"),
               Id = timeTracker.Id.ToString(),
               StartDate = timeTracker.StartDate.ToString("yyyy/MM/dd HH:mm:ss"),
               UpdatedAt = timeTracker.UpdatedAt?.ToString("yyyy/MM/dd HH:mm:ss")
            };

            return CreatedAtAction(nameof(Create), new { id = timeTracker.Id }, response);
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TaskResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Retorna todas as tasks com a opção de filtrar os time trackers por projeto e colaborador.")]
        public async Task<ActionResult<PaginatedResult<TaskResponse>>> GetTasks([FromQuery] Guid? projectId,
            [FromQuery] Guid? collaboratorId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)

        {
           
            var useCase = new GetTimeTrackersUseCase(timeTrackerRepository);

            var tasks = await useCase.Execute(projectId, collaboratorId);

            if (tasks == null || !tasks.Any())
            {
                return NotFound("Nenhuma task encontrada.");
            }

            var response = tasks.Select(task => new TaskResponse
            {
                TaskId = task.Id.ToString(),
                TaskName = task.Name,
                ProjectId = task.Project.Id.ToString(),
                ProjectName = task.Project.Name,
                Collaborators = task.TimeTrackers.Select(tt => new CollaboratorResponse
                {
                    CollaboratorId = tt.Collaborator.Id.ToString(),
                    CollaboratorName = tt.Collaborator.Name
                }).ToList()
            });

            return Ok(response);
        }
    }
}
