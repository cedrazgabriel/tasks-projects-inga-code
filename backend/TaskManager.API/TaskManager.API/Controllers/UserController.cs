using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.DTO.Request;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Cryptograph.Contracts;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.Services;
using TaskManager.Application.UseCases;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserRepository repository, IHashGenerator hashGenerator, IHashCompare hashCompare, IAuthService authService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<TokenResponse>> Register([FromBody] UserRegisterRequest request)
        {
            var useCase = new CreateUserUseCase(repository, hashGenerator, authService);

            var token = await useCase.Execute(request.Username, request.Password);

            var response = new TokenResponse
            {
                Token = token
            };

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> Login(UserRegisterRequest request)
        {
            var useCase = new AuthenticateUseCase(repository, hashCompare, authService);

            var token = await useCase.Execute(request.Username, request.Password);

            var response = new TokenResponse { Token = token };

            return Ok(response);

        }
       
    }
}
