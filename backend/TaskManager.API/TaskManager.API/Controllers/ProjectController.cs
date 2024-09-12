using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.API.DTO.Request;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.Services;
using TaskManager.Application.UseCases.Projects;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata.Ecma335;
using AutoMapper;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [SwaggerTag("Gerencia os projetos da aplicação")] 
    public class ProjectController(IProjectRepository projectRepository, ICacheService cacheService, IMapper mapper) : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResult<ProjectResponse>))]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Consultar projetos", Description = "Este endpoint é usado para consultar todos os projetos da plataforma.")]
        public async Task<ActionResult<PaginatedResult<ProjectResponse>>> GetProjects([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var useCase = new GetProjectsUseCase(projectRepository, cacheService);

            var projects = await useCase.Execute(page, pageSize);

            var response = new PaginatedResult<ProjectResponse>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = projects.TotalRecords,
                Items = mapper.Map<List<ProjectResponse>>(projects.Items)
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Consultar projeto por ID", Description = "Este endpoint é usado para consultar um projeto específico na plataforma utilizando o ID do projeto.")]
        public async Task<ActionResult<ProjectResponse>> GetProjectById(Guid id)
        {
            var useCase = new GetProjectByIdUseCase(projectRepository);

            var project = await useCase.Execute(id);

            if (project == null)
            {
                return NotFound();
            }

            var response = mapper.Map<ProjectResponse>(project);

            return Ok(response);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateProjectResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Registra um novo projeto", Description = "Este endpoint é usado para registrar um novo projeto na plataforma.")]
        public async Task<ActionResult<CreateProjectResponse>> CreateProject([FromBody] CreateProjectRequest request)
        {
            var useCase = new CreateProjectUseCase(projectRepository, cacheService);

            var project = await useCase.Execute(request.Name);

            var response = mapper.Map<CreateProjectResponse>(project);

            return CreatedAtAction(nameof(CreateProject), new { id = project.Id }, response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Atualizar projeto", Description = "Este endpoint é usado para atualizar um projeto existente na plataforma.")]
        public async Task<ActionResult<ProjectResponse>> UpdateProject(Guid id, [FromBody] UpdateProjectRequest request)
        {
            if (id == Guid.Empty || !Guid.TryParse(id.ToString(), out var projectId))
            {
                return BadRequest("The ID provided is not valid.");
            }

            var useCase = new UpdateProjectUseCase(projectRepository, cacheService);

            var project = await useCase.Execute(projectId, request.Name);

            if (project == null)
            {
                return NotFound();
            }

            var response = mapper.Map<ProjectResponse>(project);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Deletar projeto", Description = "Este endpoint é usado para deletar um projeto específico da plataforma.")]
        public async Task<ActionResult<BasicResponse>> DeleteProject(Guid id)
        {
            if (id == Guid.Empty || !Guid.TryParse(id.ToString(), out var projectId))
            {
                return BadRequest("The ID provided is not valid.");
            }

            var useCase = new DeleteProjectUseCase(projectRepository, cacheService);

            var projectDeleted = await useCase.Execute(projectId);

            var response = new BasicResponse()
            {
                message = "Project deleted successfully."
            };

            return Ok(response); 
        }

    }
}
