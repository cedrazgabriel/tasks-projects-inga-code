using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Cryptograph.Contracts;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.Services;
using TaskManager.Application.UseCases.Errors;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases
{
    public class CreateUserUseCase (IUserRepository usersRepository, IHashGenerator hashGenerator, IAuthService authService)
    {
        public async Task<string> Execute(string username, string password)
        {
            var userWithSameUsername = await usersRepository.FindByUserNameAsync(username);

            if (userWithSameUsername != null) {
                throw new UserAlreadyExistsError();
            }

            var hashedPassword = await hashGenerator.HashAsync(password);

            var user = new User(username, hashedPassword);

            await usersRepository.CreateAsync(user);

            var jwtToken = await authService.GenerateJwtToken(user.UserName);

            return jwtToken;
        }
    }
}
