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

            var timeTracker = await useCase.Execute(userGuid,
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
        public async Task<ActionResult<PaginatedResult<TimeTrackerResponse>>> GetTasks([FromQuery] Guid? projectId,
            [FromQuery] Guid? collaboratorId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)

        {
           
            var useCase = new GetTimeTrackersUseCase(timeTrackerRepository);

            var tasks = await useCase.Execute(projectId, collaboratorId, page, pageSize);


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
                    EndDate = tt.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    StartDate = tt.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    TaskId = tt.TaskId.ToString(),
                    TaskName = tt.Task.Name,
                    UpdatedAt = tt.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")
                }).ToList()
            };
                
             
            return Ok(response);
        }
    }
}
