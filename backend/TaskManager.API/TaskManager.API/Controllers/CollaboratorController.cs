using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.UseCases.Collaborators;
using TaskManager.Application.UseCases.Projects;
using TaskManager.Infrastructure.Persistence.Repositories;

namespace TaskManager.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CollaboratorController(ICollaboratorRepository collaboratorRepository, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CollaboratorResponse))]
    [Produces("application/json")]
    [SwaggerOperation(Summary = "Consultar colaboradores", Description = "Este endpoint é usado para consultar todos os colaboradores da plataforma.")]
    public async Task<ActionResult<List<CollaboratorResponse>>> GetCollaborators()
    {
        var useCase = new GetCollaboratorsUseCase(collaboratorRepository);

        var collaborators = await useCase.Execute();

        var response = mapper.Map<List<CollaboratorResponse>>(collaborators);

        return Ok(response);
    }
}
