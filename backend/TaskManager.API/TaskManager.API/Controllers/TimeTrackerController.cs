using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TaskManager.API.DTO.Request;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.UseCases.Tasks;
using TaskManager.Application.UseCases.TimeTrackers;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [SwaggerTag("Gerencia os tempos relacionados a uma task da aplicação")]
    public class TimeTrackerController(ITaskRepository taskRepository,
        ITimeTrackerRepository timeTrackerRepository,
        ICollaboratorRepository collaboratorRepository,
        IMapper mapper) : ControllerBase
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
                request.TimeZoneId);

            var response = mapper.Map<TimeTrackerResponse>(timeTracker);

            return CreatedAtAction(nameof(Create), new { id = timeTracker.Id }, response);
        }


        [HttpPost("{timeTrackerId}/stop")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TimeTrackerResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Finalizar time tracker", Description = "Finaliza um time tracker de uma task específica.")]
        public async Task<ActionResult<TimeTrackerResponse>> Finalizate(Guid timeTrackerId)
        {
            var useCase = new StopTimeTrackerUseCase(timeTrackerRepository,taskRepository);

            var timeTracker = await useCase.Execute(timeTrackerId);

            var response = mapper.Map<TimeTrackerResponse>(timeTracker);

            return response;
        }

        [HttpGet("{taskId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TaskResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Retorna todas as tasks com a opção de filtrar os time trackers por tasks.")]
        public async Task<ActionResult<PaginatedResult<TimeTrackerResponse>>> GetTimeTrackersByTaskId(Guid taskId, [FromQuery] Guid? collaboratorId, [FromQuery] int page = 1,[FromQuery] int pageSize = 10)
        {

            var useCase = new GetTimeTrackersByTaskIdUseCase(timeTrackerRepository);

            var tasks = await useCase.Execute(taskId, page, pageSize, collaboratorId);

            var response = new PaginatedResult<TimeTrackerResponse>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = tasks.TotalRecords,
                Items = mapper.Map<List<TimeTrackerResponse>>(tasks.Items)
            };

            return Ok(response);
        }

        [HttpGet("metrics")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MetricsResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Retorna o total de horas gastas do usuário no mês, semana e dia.")]
        public async Task<ActionResult<MetricsResponse>> GetMetrics()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Usuário não autenticado.");
            }

            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return BadRequest("ID de usuário inválido.");
            }


            var useCase = new GetMetricsUseCase(timeTrackerRepository, collaboratorRepository);

            var metrics = await useCase.Execute(userGuid);
          
            var response = mapper.Map<MetricsResponse>(metrics);

            return Ok(response);
        }
    }
}
