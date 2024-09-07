using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.API.DTO.Request;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Cryptograph.Contracts;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.Services;
using TaskManager.Application.UseCases.Users;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Gerencia os usuários e autenticação da aplicação")]
    public class UserController(IUserRepository userRepository,
        ICollaboratorRepository collaboratorRepository,
        IHashGenerator hashGenerator,
        IHashCompare hashCompare,
        IAuthService authService) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Registra um novo usuário", Description = "Este endpoint é usado para registrar um novo usuário na plataforma.")]
        public async Task<ActionResult<TokenResponse>> Register([FromBody] UserRegisterRequest request)
        {
            var useCase = new CreateUserUseCase(userRepository, collaboratorRepository, hashGenerator, authService);

            var token = await useCase.Execute(request.Username, request.Password);

            var response = new TokenResponse
            {
                Token = token
            };

            return Ok(response);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Realiza o login", Description = "Este endpoint é usado para realizar o login na plataforma")]
        public async Task<ActionResult<TokenResponse>> Login(UserRegisterRequest request)
        {
            var useCase = new AuthenticateUseCase(userRepository, hashCompare, authService);

            var token = await useCase.Execute(request.Username, request.Password);

            var response = new TokenResponse { Token = token };

            return Ok(response);

        }

    }
}
